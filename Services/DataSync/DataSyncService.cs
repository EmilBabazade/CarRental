using Data.Repos.Cars;
using Domain.RentalCar;
using Microsoft.Extensions.Logging;

using Services.RentalAPIsClients;
using Services.RentalAPIsClients.BestRentals;
using Services.RentalAPIsClients.NorthernRentalsClient;
using Services.RentalAPIsClients.SouthRentals;

namespace Services.DataSync;
public class DataSyncService(BestRentalsClient bestRentalsClient, NorthernRentalsClient northernRentalsClient, SouthRentalsClient southRentalsClient,
    ICarsRepo carsRepo, ILogger<DataSyncService> logger) : IDataSyncService
{
    private readonly IRentalClient _bestRentalsClient = bestRentalsClient;
    private readonly NorthernRentalsClient _northernRentalsClient = northernRentalsClient;
    private readonly SouthRentalsClient _southRentalsClient = southRentalsClient;
    private readonly ICarsRepo _carsRepo = carsRepo;
    private readonly ILogger<DataSyncService> _logger = logger;

    public async Task SynchronizeDataAsync(CancellationToken cancellationToken = default)
    {
        var rentals = new List<Car>();
        // I assumed each uri should act like a different api since task called them seperate apis
        // otherwise i would just put all three methods in the same client 

        rentals.AddRange(await GetCarsAsync(_bestRentalsClient, "best", cancellationToken)); ;
        rentals.AddRange(await GetCarsAsync(_northernRentalsClient, "north", cancellationToken));
        rentals.AddRange(await GetCarsAsync(_southRentalsClient, "south", cancellationToken));
        await _carsRepo.BulkUpsertAsync(rentals, cancellationToken);
    }

    private async Task<IEnumerable<Car>> GetCarsAsync(IRentalClient client, string name = "",
        CancellationToken cancellationToken = default)
    {
        try
        {
            return await client.GetCarsAsync(cancellationToken);
        }
        catch (OperationCanceledException ex)
        {
            // send email or somethin
            _logger.LogError(ex, name + " rental client is borked");
            return [];
        }
    }
}
