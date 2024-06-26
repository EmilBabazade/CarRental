using AutoMapper;
using Data.Caching;
using Data.Entities;
using Domain.RentalCar;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Data.Repos.Cars;
public class CarsRepo(DataContext dataContext, IMapper mapper, ICache<IEnumerable<Car>> carCache) : ICarsRepo
{
    private readonly DataContext _dataContext = dataContext;
    private readonly IMapper _mapper = mapper;
    private readonly ICache<IEnumerable<Car>> _carCache = carCache;

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

    public async Task<IEnumerable<Car>> GetAllCars(FilterDTO? filter = null, CancellationToken cancellationToken = default)
    {
        var cacheKey = JsonSerializer.Serialize(filter);
        var cars = await _carCache.GetAsync(cacheKey, cancellationToken);
        if (cars != null) return cars;
        if (filter == null)
        {
            var carEntities = await _dataContext.Cars.ToArrayAsync(cancellationToken);
            cars = _mapper.Map<IEnumerable<Car>>(carEntities);
            await _carCache.SetAsync(cacheKey, cars, cancellationToken);
            // TODO: remove
            await Task.Delay(5000);
            return cars;
        }

        IQueryable<CarEntity> query = _dataContext.Cars;

        // TODO: fix hard dependency on FilterDTO
        if (filter.Id != null)
        {
            query = query.Where(c => c.Id == filter.Id);
        }

        if (filter.ImageURL != null)
        {
            query = query.Where(c => c.ImageURL.ToLower() == filter.ImageURL.ToLower());
        }

        if (filter.Currency != null)
        {
            query = query.Where(c => c.Currency.ToLower() == filter.Currency.ToLower());
        }

        if (filter.Cost != null)
        {
            query = query.Where(c => c.Cost == filter.Cost);
        }

        if (filter.LogoURL != null)
        {
            query = query.Where(c => c.LogoURL.ToLower() == filter.LogoURL.ToLower());
        }

        if (filter.QuoteNumber != null)
        {
            query = query.Where(c => c.QuoteNumber.ToLower() == filter.QuoteNumber.ToLower());
        }

        if (filter.Sipp != null)
        {
            query = query.Where(c => c.Sipp.ToLower() == filter.Sipp.ToLower());
        }

        if (filter.Vehicle != null)
        {
            query = query.Where(c => c.Vehicle.ToLower() == filter.Vehicle.ToLower());
        }

        var filteredCars = await query.ToArrayAsync(cancellationToken);
        var res = _mapper.Map<IEnumerable<Car>>(filteredCars);
        await _carCache.SetAsync(cacheKey, res, cancellationToken);
        // TODO: remove
        await Task.Delay(5000, cancellationToken);
        return res;
    }
}
