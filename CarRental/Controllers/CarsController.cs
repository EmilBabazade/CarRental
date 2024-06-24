using Clients.BestRentals;
using Clients.NorthernRentalsClient;
using Clients.SouthRentals;
using Data.Repos;
using Domain.RentalCar;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CarsController(ICarsRepo carsRepo, ISouthRentalsClient southRentalsClient, INorthernRentalsClient northernRentalsClient,
    IBestRentalsClient bestRentalsClient) : ControllerBase
{
    private readonly ICarsRepo _carsRepo = carsRepo;
    private readonly ISouthRentalsClient _southRentalsClient = southRentalsClient;
    private readonly INorthernRentalsClient _northernRentalsClient = northernRentalsClient;
    private readonly IBestRentalsClient _bestRentalsClient = bestRentalsClient;

    [HttpGet]
    [Route("SyncData")]
    public async Task<ActionResult> SyncData(CancellationToken cancellationToken = default)
    {
        // I assumed each uri should act like a different api (since task called them seperate apis) so I didn't base them on same interface
        // otherwise i would go with that ( or better just put all three methods in the same client )
        var rentals = new List<Car>();
        rentals.AddRange(await _bestRentalsClient.GetCarsAsync(cancellationToken));
        rentals.AddRange(await _northernRentalsClient.GetCarsAsync(cancellationToken));
        rentals.AddRange(await _southRentalsClient.GetCarsAsync(cancellationToken));

        await _carsRepo.BulkUpsertAsync(rentals, cancellationToken);
        return Ok();
    }

    [HttpGet]
    [Route("GetAll")]
    public async Task<ActionResult<IEnumerable<Car>>> GetAllCars(CancellationToken cancellationToken = default)
    {
        return Ok(await _carsRepo.GetAllCars(cancellationToken));
    }
}
