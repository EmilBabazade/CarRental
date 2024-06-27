using AutoMapper;
using Data.Caching;
using Data.Entities;
using Data.MappingProfiles;
using Data.Repos.Cars;
using Domain.RentalCar;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Data.Test;

public class CarsRepoTests
{
    private DataContext _context;
    private IMapper _mapper;

    [SetUp]
    public void Setup()
    {
        var testMapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CarMappingProfile>();
        });
        _mapper = testMapperConfig.CreateMapper();

        var testContextOptions = new DbContextOptionsBuilder<DataContext>()
                                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                                    .Options;
        _context = new DataContext(testContextOptions);
    }

    [TearDown]
    public void Cleanup()
    {
        _context.Dispose();
    }

    [Test]
    public async Task TestGetAllCarsReturnsAllFromCache()
    {
        IEnumerable<Car> cars = new Car[]
        {
            new() {
                Cost = 123,
                Currency = "bob",
                ImageURL = "bob",
                LogoURL = "bob",
                QuoteNumber = "bob",
                Sipp = "bob",
                Vehicle = "bob"
            },
            new() {
                Cost = 1234,
                Currency = "bob5",
                ImageURL = "bob6",
                LogoURL = "bob5",
                QuoteNumber = "bob4",
                Sipp = "bob3",
                Vehicle = "bob3"
            }
        };

        var cacheMock = new Mock<ICache<IEnumerable<Car>>>();
        cacheMock
            .Setup(m => m.GetAsync("null", It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(cars));

        var repo = new CarsRepo(_context, _mapper, cacheMock.Object);
        var result = await repo.GetAllCars();

        result.Should().BeEquivalentTo(cars);
    }

    [Test]
    public async Task TestGetAllCarsReturnsAllFromDB()
    {
        IEnumerable<Car> cars = new Car[]
        {
            new() {
                Cost = 123,
                Currency = "bob",
                ImageURL = "bob",
                LogoURL = "bob",
                QuoteNumber = "bob",
                Sipp = "bob",
                Vehicle = "bob"
            },
            new() {
                Cost = 1234,
                Currency = "bob5",
                ImageURL = "bob6",
                LogoURL = "bob5",
                QuoteNumber = "bob4",
                Sipp = "bob3",
                Vehicle = "bob3"
            }
        };

        var cacheMock = new Mock<ICache<IEnumerable<Car>>>();
        cacheMock
            .Setup(m => m.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult<IEnumerable<Car>>(null));

        _context.AddRange(_mapper.Map<IEnumerable<CarEntity>>(cars));
        await _context.SaveChangesAsync();

        var repo = new CarsRepo(_context, _mapper, cacheMock.Object);
        var result = await repo.GetAllCars();

        result.Should().BeEquivalentTo(cars);
    }
}