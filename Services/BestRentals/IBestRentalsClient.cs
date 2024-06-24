using Domain.RentalCar;

namespace Services.BestRentals;

public interface IBestRentalsClient
{
    Task<IEnumerable<Car>> GetCarsAsync(CancellationToken cancellationToken = default);
}