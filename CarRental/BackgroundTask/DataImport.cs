
using AutoMapper;
using Data.MappingProfiles;
using Data.Repos.Cars;
using Domain.RentalCar;
using Services.BestRentals;
using Services.NorthernRentalsClient;
using Services.SouthRentals;

namespace CarRental.BackgroundTask;

public class DataImport : IHostedService, IDisposable
{
    private Timer? _timer = null;
    public void Dispose()
    {
        _timer?.Dispose();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
    }

    private void DoWork(object? _)
    {
        // TODO: put into a seperate service
        var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarMappingProfile>();
                cfg.AddProfile<BestRentalsResponseMappingProfile>();
                cfg.AddProfile<NorthernRentalsResponseMappingProfile>();
                cfg.AddProfile<SouthRentalsResponseMappingProfile>();
            }).CreateMapper();
        var inMemorySettings = new Dictionary<string, string> {
                {"NorthernRentalsURL", "https://suppliers-test.dev-dch.com/api/v1/NorthernRentals/GetRates"},
                {"SouthernRentalsURL", "https://suppliers-test.dev-dch.com/api/v1/SouthRentals/Quotes"},
                { "BestRentalsURL", "https://suppliers-test.dev-dch.com/api/v1/BestRentals/AvailableOffers"},
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        using var client = new HttpClient();
        var bestRentalClient = new BestRentalsClient(configuration, client, mapper);
        var northRentalClient = new NorthernRentalsClient(configuration, client, mapper);
        var southRentalClient = new SouthRentalsClient(configuration, client, mapper);

        var rentals = new List<Car>();
        rentals.AddRange(bestRentalClient.GetCarsAsync().Result);
        rentals.AddRange(northRentalClient.GetCarsAsync().Result);
        rentals.AddRange(southRentalClient.GetCarsAsync().Result);

        var repo = new CarsRepo()
        await _carsRepo.BulkUpsertAsync(rentals, cancellationToken);
    }
}
