using Domain.RentalCar;

namespace Services.RentalAPIsClients;

public interface IRentalClient
{
    Task<IEnumerable<Car>> GetCarsAsync(CancellationToken cancellationToken = default);
}