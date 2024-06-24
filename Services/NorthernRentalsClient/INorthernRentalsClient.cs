using Domain.RentalCar;

namespace Clients.NorthernRentalsClient;

public interface INorthernRentalsClient
{
    Task<IEnumerable<Car>> GetCarsAsync(CancellationToken cancellationToken = default);
}