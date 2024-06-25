using Data.Repos.Cars;
using Domain.RentalCar;
using Services.BestRentals;
using Services.NorthernRentalsClient;
using Services.SouthRentals;

namespace Services.DataSync;
public class DataSyncService(IBestRentalsClient bestRentalsClient, INorthernRentalsClient northernRentalsClient, ISouthRentalsClient southRentalsClient,
    ICarsRepo carsRepo) : IDataSyncService
{
    private readonly IBestRentalsClient _bestRentalsClient = bestRentalsClient;
    private readonly INorthernRentalsClient _northernRentalsClient = northernRentalsClient;
    private readonly ISouthRentalsClient _southRentalsClient = southRentalsClient;
    private readonly ICarsRepo _carsRepo = carsRepo;

    public async Task SynchronizeDataAsync(CancellationToken cancellationToken = default)
    {
        var rentals = new List<Car>();
        // I assumed each uri should act like a different api (since task called them seperate apis) so I didn't base them on same interface
        // otherwise i would go with that ( or better just put all three methods in the same client )
        rentals.AddRange(await _bestRentalsClient.GetCarsAsync(cancellationToken));
        rentals.AddRange(await _northernRentalsClient.GetCarsAsync(cancellationToken));
        rentals.AddRange(await _southRentalsClient.GetCarsAsync(cancellationToken));

        await _carsRepo.BulkUpsertAsync(rentals, cancellationToken);
    }
}
