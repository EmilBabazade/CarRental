using Domain.RentalCar;

namespace Data.Repos;
public interface ICarsRepo
{
    Task BulkUpsertAsync(IEnumerable<Car> cars, CancellationToken cancellationToken = default);

    Task<IEnumerable<Car>> GetAllCars(CancellationToken cancellationToken = default);
}