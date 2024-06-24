using AutoMapper;
using Data.Entities;
using Domain.RentalCar;
using Microsoft.EntityFrameworkCore;

namespace Data.Repos;
public class CarsRepo(DataContext dataContext, IMapper mapper) : ICarsRepo
{
    private readonly DataContext _dataContext = dataContext;
    private readonly IMapper _mapper = mapper;

    public async Task BulkUpsertAsync(IEnumerable<Car> cars, CancellationToken cancellationToken = default)
    {
        var quoteNumbers = cars.Select(c => c.QuoteNumber);
        var existingEntities = await _dataContext.Cars
            .Where(c => quoteNumbers.Contains(c.QuoteNumber))
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var newCars = cars.Where(c => !existingEntities.Exists(ce => ce.QuoteNumber == c.QuoteNumber));
        _dataContext.AddRange(_mapper.Map<IEnumerable<CarEntity>>(newCars));

        foreach (var existingCar in existingEntities)
        {
            var updatedCar = _mapper.Map<CarEntity>(cars.First(c => c.QuoteNumber == existingCar.QuoteNumber));
            updatedCar.Id = existingCar.Id;
            _dataContext.Update(updatedCar);
        }
        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<Car>> GetAllCars(CancellationToken cancellationToken = default)
    {
        var cars = await _dataContext.Cars.ToArrayAsync(cancellationToken);
        return _mapper.Map<IEnumerable<Car>>(cars);
    }
}
