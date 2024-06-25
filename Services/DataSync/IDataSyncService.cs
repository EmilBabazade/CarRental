
namespace Services.DataSync;

public interface IDataSyncService
{
    Task SynchronizeDataAsync(CancellationToken cancellationToken = default);
}