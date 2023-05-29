
using System;
using System.Text;
using System.Text.Json;
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
using Amazon.Runtime.Internal;

public class GetEmergencyEndpoint : EndpointWithoutRequest<List<GetEmergenciesResponse>>
{
    private readonly IHttpClientFactory _httpClientFactory;

    private List<string> colorList = new();

    public override void Configure()
    {
        Get("/api/emergencies");
        AllowAnonymous();
    }

    public GetEmergencyEndpoint(IHttpClientFactory httpClientFactory)
    {
        colorList = new List<string> {
            "#00ff00",
            "#33ff00",
            "#66ff00",
            "#99ff00",
            "#ccff00",
            "#ffff00",
            "#ffcc00",
            "#ff9900",
            "#ff6600",
            "#ff3300"
        };

        _httpClientFactory = httpClientFactory;
    }

    public override async Task HandleAsync(CancellationToken c)
    {
        using var context = new ApplicationContext();
        var random = new Random();
        var emergencies = await context.Emergencies.ToListAsync();

        var responseObjectList = new List<GetEmergenciesResponse>();

        foreach (var e in emergencies)
        {
            responseObjectList.Add(new GetEmergenciesResponse
            {
                Status = colorList[random.Next(9)],
                Longidute = e.Longidute,
                Latidute = e.Latidute
            });
        }

        var response = new GetEmergenciesResponse
        {
            Status = "okey"
        };

        var json = JsonSerializer.Serialize(response);

        await SendAsync(
                    responseObjectList,
                    cancellation: c
                );
    }
}

public record GetEmergenciesResponse
{
    public string Status { get; init; }
    public float Longidute { get; init; }
    public float Latidute { get; init; }
    public string Name { get; init; }
}



