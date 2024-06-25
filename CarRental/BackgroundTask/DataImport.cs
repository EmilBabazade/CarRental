
using AutoMapper;
using Data.MappingProfiles;
using Data.Repos.Cars;
using Domain.RentalCar;
using Services.BestRentals;
using Services.DataSync;
using Services.NorthernRentalsClient;
using Services.SouthRentals;

namespace CarRental.BackgroundTask;

public class DataImport(IDataSyncService dataSyncService, IConfiguration configuration) : IHostedService, IDisposable
{
    private readonly IDataSyncService _dataSyncService = dataSyncService;
    private readonly IConfiguration _configuration = configuration;
    private Timer? _timer = null;

    public void Dispose()
    {
        _timer?.Dispose();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(int.Parse(_configuration["CacheTTLInMinutes"])));
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
    }

    private async void DoWork(object? _)
    {
        await _dataSyncService.SynchronizeDataAsync();
    }
}
