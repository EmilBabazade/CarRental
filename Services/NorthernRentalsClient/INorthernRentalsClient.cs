using Domain.RentalCar;

namespace Services.NorthernRentalsClient;

public interface INorthernRentalsClient
{
    Task<IEnumerable<Car>> GetCarsAsync(CancellationToken cancellationToken = default);
}