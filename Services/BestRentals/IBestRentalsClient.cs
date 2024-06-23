using Domain.RentalCar;

namespace Clients.BestRentals;

public interface IBestRentalsClient
{
    Task<IEnumerable<Car>> GetCars(CancellationToken cancellationToken = default);
}