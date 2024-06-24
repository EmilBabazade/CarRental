using AutoMapper;
using Domain.RentalCar;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace Services.NorthernRentalsClient;
public class NorthernRentalsClient(IConfiguration configuration, HttpClient httpClient, IMapper mapper) : INorthernRentalsClient
{
    private readonly IConfiguration _configuration = configuration;
    private readonly HttpClient _httpClient = httpClient;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<Car>> GetCarsAsync(CancellationToken cancellationToken = default)
    {
        var response =
            await _httpClient.GetFromJsonAsync<IEnumerable<NorthernRentalsResponse>>(_configuration["NorthernRentalsURL"], cancellationToken);
        return _mapper.Map<IEnumerable<Car>>(response);
    }
}
