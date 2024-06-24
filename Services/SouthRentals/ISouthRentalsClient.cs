using Domain.RentalCar;

namespace Services.SouthRentals;
public interface ISouthRentalsClient
{
    Task<IEnumerable<Car>> GetCarsAsync(CancellationToken cancellationToken = default);
}