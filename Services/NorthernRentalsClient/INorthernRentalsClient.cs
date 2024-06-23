using Domain.RentalCar;

namespace Clients.NorthernRentalsClient;

public interface INorthernRentalsClient
{
    Task<IEnumerable<Car>?> GetCars(CancellationToken cancellationToken = default);
}