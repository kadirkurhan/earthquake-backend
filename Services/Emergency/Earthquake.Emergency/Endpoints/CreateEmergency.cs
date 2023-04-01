
using System;
using System.Text;
using System.Text.Json;
using Earthquake.Emergency.Entities;
using Microsoft.Net.Http.Headers;
using System.Net.Http;
using Amazon.SecretsManager;

public record CreateEmergency
{
    public Int64 SenderUserId { get; init; }
    public Int64 TransactionId { get; init; }
    public VictimsData Location { get; init; }
}

public record VictimsData
{
    public Int64 UserId { get; init; }
    public float Longidute { get; init; }
    public float Latidute { get; init; }
}

public record SuccessResultResponse
{
    public bool isActive { get; init; }
}

// Lambda model
public class RequestModel
{
    public string SenderName { get; set; }
    public IEnumerable<Message> Receivers { get; set; }
}

public class Message
{
    public string ReceiverName { get; set; }
    public string PhoneNumber { get; set; }
    public string Longidute { get; set; }
    public string Latidute { get; set; }
    public string Time { get; set; }
}

public class EmergencyEndpoint : Endpoint<List<CreateEmergency>>
{
    private readonly IHttpClientFactory _httpClientFactory;

    public override void Configure()
    {
        Post("/api/emergency/create");
        AllowAnonymous();
    }

    public EmergencyEndpoint(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public override async Task HandleAsync(List<CreateEmergency> req, CancellationToken ct)
    {
        var dbQueryResult = new Emergency[] {
        new Emergency { Id = 1, IsActive = true, UserId = 1, X = (float)1.22, Y = (float)1.5 },
        new Emergency { Id = 2, IsActive = true, UserId = 2, X = (float)1.22, Y = (float)1.5 },
        new Emergency { Id = 3, IsActive = true, UserId = 3, X = (float)1.22, Y = (float)1.5 }
    }.Select(x => x.UserId).ToList();

        var checkingUsers = req.Select(x => x.Location);
        var allVictimUsersIds = checkingUsers.Select(x => x.UserId);

        var notExistUserIds = allVictimUsersIds.Where(x => !dbQueryResult.Contains(x));

        const string lambdaUrl = "https://w7ug5epr25.execute-api.eu-central-1.amazonaws.com/prod/api/notification";

        var client = _httpClientFactory.CreateClient();

        var list = new List<Message>() { new Message { PhoneNumber = "+905457624338",Latidute ="123",Longidute ="123",ReceiverName ="yas", Time ="12.12.12"}
          };

        var request = new RequestModel
        {
            SenderName = "kadir",
            Receivers = list.AsEnumerable<Message>()
        };

        var httpRequestMessage = new HttpRequestMessage(System.Net.Http.HttpMethod.Post, lambdaUrl)
        {
            Headers =
                {
                    { HeaderNames.Accept, "application/json" },
                },
            Content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json")
        };

        var result = await client.SendAsync(httpRequestMessage);

        await SendAsync(notExistUserIds);
    }
}



