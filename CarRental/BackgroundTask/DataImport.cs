using Services.DataSync;

namespace CarRental.BackgroundTask;

public class DataImport(IServiceProvider serviceProvider, IConfiguration configuration) : IHostedService, IDisposable
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly IConfiguration _configuration = configuration;
    private Timer? _timer = null;

    public void Dispose()
    {
        _timer?.Dispose();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var expiresInMinutes = _configuration["CacheTTLInMinutes"] == null ? 30 : int.Parse(_configuration["CacheTTLInMinutes"]);
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(expiresInMinutes));
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);
    }

    private async void DoWork(object? _)
    {
        using var scope = _serviceProvider.CreateScope();
        var dataSyncService = scope.ServiceProvider.GetService<IDataSyncService>();
        // TODO: handle null dataSyncServiec
        await dataSyncService.SynchronizeDataAsync();
    }
}
