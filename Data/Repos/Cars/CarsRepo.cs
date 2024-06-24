using AutoMapper;
using Data.Entities;
using Domain.RentalCar;
using Microsoft.EntityFrameworkCore;
using Services.InMemory;
using System.Text.Json;

namespace Data.Repos.Cars;
public class CarsRepo(DataContext dataContext, IMapper mapper, InMemoryCache<IEnumerable<Car>> carCache) : ICarsRepo
{
    private readonly DataContext _dataContext = dataContext;
    private readonly IMapper _mapper = mapper;
    private readonly InMemoryCache<IEnumerable<Car>> _carCache = carCache;

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
        var cars = _carCache.Get(cacheKey);
        if (cars != null) return cars;
        if (filter == null)
        {
            var carEntities = await _dataContext.Cars.ToArrayAsync(cancellationToken);
            cars = _mapper.Map<IEnumerable<Car>>(carEntities);
            _carCache.Set(cacheKey, cars);
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
            query = query.Where(c => c.ImageURL == filter.ImageURL);
        }

        if (filter.Currency != null)
        {
            query = query.Where(c => c.Currency == filter.Currency);
        }

        if (filter.Cost != null)
        {
            query = query.Where(c => c.Cost == filter.Cost);
        }

        if (filter.LogoURL != null)
        {
            query = query.Where(c => c.LogoURL == filter.LogoURL);
        }

        if (filter.QuoteNumber != null)
        {
            query = query.Where(c => c.QuoteNumber == filter.QuoteNumber);
        }

        if (filter.Sipp != null)
        {
            query = query.Where(c => c.Sipp == filter.Sipp);
        }

        if (filter.Vehicle != null)
        {
            query = query.Where(c => c.Vehicle == filter.Vehicle);
        }

        var filteredCars = await query.ToArrayAsync(cancellationToken);
        var res = _mapper.Map<IEnumerable<Car>>(filteredCars);
        _carCache.Set(cacheKey, res);
        // TODO: remove
        await Task.Delay(5000);
        return res;
    }
}
