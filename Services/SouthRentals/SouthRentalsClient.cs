using AutoMapper;
using Domain.RentalCar;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace Clients.SouthRentals;
public class SouthRentalsClient(IConfiguration configuration, HttpClient httpClient, IMapper mapper) : ISouthRentalsClient
{
    private readonly IConfiguration _configuration = configuration;
    private readonly HttpClient _httpClient = httpClient;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<Car>> GetCarsAsync(CancellationToken cancellationToken = default)
    {
        var response =
            await _httpClient.GetFromJsonAsync<IEnumerable<SouthRentalsResponse>>(_configuration["SouthernRentalsURL"], cancellationToken);
        return _mapper.Map<IEnumerable<Car>>(response);
    }
}
