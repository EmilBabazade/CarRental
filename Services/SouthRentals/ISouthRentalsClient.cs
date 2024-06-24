using Domain.RentalCar;

namespace Clients.SouthRentals;
public interface ISouthRentalsClient
{
    Task<IEnumerable<Car>> GetCarsAsync(CancellationToken cancellationToken = default);
}