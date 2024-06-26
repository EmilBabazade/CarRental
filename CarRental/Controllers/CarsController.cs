using Data.Repos.Cars;
using Domain.RentalCar;
using Microsoft.AspNetCore.Mvc;
using Services.DataSync;
using Services.RentalAPIsClients.BestRentals;
using Services.RentalAPIsClients.NorthernRentalsClient;
using Services.RentalAPIsClients.SouthRentals;

namespace CarRental.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CarsController(ICarsRepo carsRepo, SouthRentalsClient southRentalsClient, NorthernRentalsClient northernRentalsClient,
    BestRentalsClient bestRentalsClient, IDataSyncService dataSyncService) : ControllerBase
{
    private readonly ICarsRepo _carsRepo = carsRepo;
    private readonly SouthRentalsClient _southRentalsClient = southRentalsClient;
    private readonly NorthernRentalsClient _northernRentalsClient = northernRentalsClient;
    private readonly BestRentalsClient _bestRentalsClient = bestRentalsClient;
    private readonly IDataSyncService _dataSyncService = dataSyncService;

    [HttpGet]
    [Route("SyncData")]
    public async Task<ActionResult> SyncData(CancellationToken cancellationToken = default)
    {
        await _dataSyncService.SynchronizeDataAsync(cancellationToken);
        return Ok();
    }

    [HttpPost]
    [Route("GetAll")]
    public async Task<ActionResult<IEnumerable<Car>>> GetAllCars([FromBody] FilterDTO? filterDTO, CancellationToken cancellationToken = default)
    {
        return Ok(await _carsRepo.GetAllCars(filterDTO, cancellationToken));
    }
}
