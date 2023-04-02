
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

        //var allVictimIds = req.Receivers.SelectMany(x => x.UserId, (user) => user.UserId).ToList();

        //await context.Emergencies.Where(x=>req.Contains(x.UserId));

        foreach (var item in req.Receivers)
        {
            await context.Emergencies.AddAsync(new EmergencyEntity
            {
                Id = random.Next(),
                Longidute = item.Longidute,
                Latidute = item.Latidute,
                ReportedByUserId = req.SenderUserId,
                UserId = random.Next()
            });
        }

        await context.SaveChangesAsync();


        //var checkingUsers = req.Select(x => x.Location);
        //var allVictimUsersIds = checkingUsers.Select(x => x.UserId);

        //var notExistUserIds = allVictimUsersIds.Where(x => !dbQueryResult.Contains(x));

        const string lambdaGatewayUrl = "https://w7ug5epr25.execute-api.eu-central-1.amazonaws.com/prod/api/notification";

        var client = _httpClientFactory.CreateClient();

        var list = new List<MessageDto>() { new MessageDto { PhoneNumber = "+905457624338",Latidute ="123",Longidute ="123",ReceiverName ="yas", Time ="12.12.12"}
          };

        var request = new CreateRequestDto
        {
            SenderName = "kadir",
            Receivers = list.AsEnumerable<MessageDto>()
        };

        var httpRequestMessage = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, lambdaGatewayUrl)
        {
            Headers =
                {
                    { HeaderNames.Accept, "application/json" },
                },
            Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
        };

        var result = await client.SendAsync(httpRequestMessage);

        await SendAsync(200);
    }
}



