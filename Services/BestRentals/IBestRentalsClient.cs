using Domain.RentalCar;

namespace Clients.BestRentals;

public interface IBestRentalsClient
{
    Task<IEnumerable<Car>> GetCarsAsync(CancellationToken cancellationToken = default);
}