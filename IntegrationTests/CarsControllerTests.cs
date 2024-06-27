using AutoMapper;
using Data;
using Data.Entities;
using Domain.RentalCar;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace IntegrationTests;

public class CarsControllerTests
{
    private readonly CustomWebApplicationFactory<Program> _factory;
    private const string _urlRoot = "api/Cars";

    public CarsControllerTests()
    {
        _factory = new();
    }

    private async Task<HttpResponseMessage?> SendRequestAsync(string url, HttpMethod method, object? body = null)
    {
        var request = new HttpRequestMessage(method, _urlRoot + url);
        if (method != HttpMethod.Get)
        {
            var serializedBody = JsonSerializer.Serialize(body ?? new JsonObject(), new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            });
            var stringContent = new StringContent(serializedBody);
            stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            request.Content = stringContent;
        }
        return await _factory.CreateClient().SendAsync(request);
    }

    private async Task<TResult?> SendRequestAsync<TResult>(string url, HttpMethod method, object? body = null)
    {
        var response = await SendRequestAsync(url, method, body);
        var responseString = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TResult>(responseString,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter() }
            });
    }

    private DataContext GetDataContext() =>
        _factory.Services.CreateScope().ServiceProvider.GetService<DataContext>();

    private IMapper GetMapper() =>
        _factory.Services.CreateScope().ServiceProvider.GetService<IMapper>();

    [SetUp]
    public async Task Setup()
    {
        // clear db between tests
        using var dbContext = GetDataContext();
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();
    }

    [Test]
    public async Task TestGetAllWorks()
    {
        var carEntities = new CarEntity[]
        {
            new() {
                Cost = 123,
                Currency = "bob",
                ImageURL = "bob",
                LogoURL = "bob",
                QuoteNumber = "bob",
                Sipp = "bob",
                Vehicle = "bob"
            },
            new() {
                Cost = 1234,
                Currency = "bob5",
                ImageURL = "bob6",
                LogoURL = "bob5",
                QuoteNumber = "bob4",
                Sipp = "bob3",
                Vehicle = "bob3"
            }
        };

        using var context = GetDataContext();
        context.AddRange(carEntities);
        await context.SaveChangesAsync();

        var cars = await SendRequestAsync<IEnumerable<Car>>("/GetAll", HttpMethod.Post);

        cars.Should().HaveSameCount(carEntities);
        var mapper = GetMapper();
        cars.Should().BeEquivalentTo(mapper.Map<IEnumerable<Car>>(carEntities));
    }
}