using Domain.RentalCar;

namespace Clients.SouthRentals;
public interface ISouthRentalsClient
{
    Task<IEnumerable<Car>> GetCars(CancellationToken cancellationToken = default);
}