
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
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;

public class GetEmergencyEndpoint : EndpointWithoutRequest<List<GetEmergenciesResponse>>
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCache _cache;

    private List<string> colorList = new();

    public override void Configure()
    {
        Get("/api/emergencies");
        AllowAnonymous();
        ResponseCache(10); //cache for 60 seconds

    }

    public GetEmergencyEndpoint(IHttpClientFactory httpClientFactory, IMemoryCache cache)
    {
        colorList = new List<string> {
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
        _cache = cache;
    }

    public override async Task HandleAsync(CancellationToken c)
    {
        //if (_cache.TryGetValue("myCache", out List<GetEmergenciesResponse> list))
        //      await SendAsync(
        //            list,
        //            cancellation: c
        //        );

        using var context = new ApplicationContext();
        var random = new Random();
        var emergencies = await context.Emergencies.Include(x=>x.User).ToListAsync();

        var responseObjectList = new List<GetEmergenciesResponse>();

        foreach (var e in emergencies)
        {
            responseObjectList.Add(new GetEmergenciesResponse
            {
                Status = colorList[random.Next(2,7)],
                Longidute = e.Longidute,
                Latidute = e.Latidute,
                Name = e.User.Name
            });
        }

        //_cache.Set("myCache", responseObjectList, new MemoryCacheEntryOptions
        //{
        //    AbsoluteExpiration = DateTime.Now.AddSeconds(10),
        //    Priority = CacheItemPriority.Normal
        //});

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



