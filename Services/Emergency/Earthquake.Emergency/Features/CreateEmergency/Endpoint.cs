
using System;
using System.Text;
using System.Text.Json;
using Earthquake.Emergency.Entities;
using Microsoft.Net.Http.Headers;
using System.Net.Http;
using Amazon.SecretsManager;
using Earthquake.Emergency.Features.CreateEmergency;
using Earthquake.Emergency.Contexts;
using Earthquake.Emergency.Domain.Entities.Emergency;
using static Earthquake.Emergency.Features.CreateEmergency.Request;
using System.Linq;
using Earthquake.Emergency.Models.DTO.Emergency;
using Microsoft.EntityFrameworkCore;

public class CreateEmergencyEndpoint : Endpoint<RequestModel>
{
    private readonly IHttpClientFactory _httpClientFactory;
    public override void Configure()
    {
        Post("/api/emergency/create");
        AllowAnonymous();
    }

    public CreateEmergencyEndpoint(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public override async Task HandleAsync(RequestModel req, CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(req);
        using var context = new ApplicationContext();
        var random = new Random();

        var requestVictims = req.Receivers
                                .GroupBy(m => m.UserId)
                                .Select(g => g.First());

        var requestVictimIds = requestVictims.Select(x => x.UserId);

        var dbVictimIds = await context.Emergencies.Select(x=>x.UserId).ToListAsync();

        var notExistReportedUserIds = requestVictimIds.Except(dbVictimIds);
        var willSendLambdaData = new List<MessageDto>();

        var notExistUsers = req.Receivers.Where(x => notExistReportedUserIds.Contains(x.UserId));

        foreach (var item in notExistUsers)
        {
            if (!notExistReportedUserIds.Contains(item.UserId))
            {
                continue;
            }

            await context.Emergencies.AddAsync(new EmergencyEntity
            {
                Id = random.Next(),
                Longidute = item.Longidute,
                Latidute = item.Latidute,
                ReportedByUserId = req.SenderUserId,
                UserId = item.UserId
            });

            willSendLambdaData.Add(new MessageDto
            {
                Longidute = item.Longidute,
                Latidute = item.Latidute,
                PhoneNumber = item.PhoneNumber,
                Time = item.Time,
                UserId = item.UserId,
                ReceiverName = item.ReceiverName
            });
        }

        await context.SaveChangesAsync();

        var awsClient = new AmazonSecretsManagerClient();
        var lambdaGatewayUrl = await awsClient.GetSecretValueAsync(new Amazon.SecretsManager.Model.GetSecretValueRequest
        {
            SecretId = "api-gateway-notification-lambda-url"
        });

        var httpClient = _httpClientFactory.CreateClient();

        var request = new CreateRequestDto
        {
            SenderName = "kadir",
            Receivers = willSendLambdaData
        };

        if (willSendLambdaData is not null)
        {
            var httpRequestMessage = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, lambdaGatewayUrl.SecretString)
            {
                Headers =
                {
                    { HeaderNames.Accept, "application/json" },
                },
                Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
            };

            var result = await httpClient.SendAsync(httpRequestMessage);
        }
        
        await SendAsync(200);
    }
}



