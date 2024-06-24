using Domain.RentalCar;

namespace Data.Repos.Cars;
public interface ICarsRepo
{
    Task BulkUpsertAsync(IEnumerable<Car> cars, CancellationToken cancellationToken = default);

    Task<IEnumerable<Car>> GetAllCars(FilterDTO? filter = null, CancellationToken cancellationToken = default);
}