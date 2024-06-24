using AutoMapper;
using Domain.RentalCar;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace Services.BestRentals;
public class BestRentalsClient(IConfiguration configuration, HttpClient httpClient, IMapper mapper) : IBestRentalsClient
{
    private readonly IConfiguration _configuration = configuration;
    private readonly HttpClient _httpClient = httpClient;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<Car>> GetCarsAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<BestRentalsResponse>>(_configuration["BestRentalsURL"], cancellationToken);
        return _mapper.Map<IEnumerable<Car>>(response);
    }
}
