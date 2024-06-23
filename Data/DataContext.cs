using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data;
public class DataContext(DbContextOptions options) : DbContext(options)
{
    public virtual DbSet<CarEntity> Cars { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder
        //    .LogTo(Console.WriteLine)
        //    .EnableSensitiveDataLogging()
        //    .EnableDetailedErrors();
        base.OnConfiguring(optionsBuilder);
    }
}
