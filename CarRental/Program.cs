using CarRental.BackgroundTask;
using Data;
using Data.Caching;
using Data.MappingProfiles;
using Data.Repos.Cars;
using Domain.RentalCar;
using Microsoft.EntityFrameworkCore;
using Services.DataSync;
using Services.RentalAPIsClients.BestRentals;
using Services.RentalAPIsClients.NorthernRentalsClient;
using Services.RentalAPIsClients.SouthRentals;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<CarMappingProfile>();
    cfg.AddProfile<BestRentalsResponseMappingProfile>();
    cfg.AddProfile<NorthernRentalsResponseMappingProfile>();
    cfg.AddProfile<SouthRentalsResponseMappingProfile>();
});
builder.Services.AddDbContext<DataContext>(options => options.UseSqlite("Data Source = Database.db"));
builder.Services.AddHttpClient<SouthRentalsClient>();
builder.Services.AddHttpClient<NorthernRentalsClient>();
builder.Services.AddHttpClient<BestRentalsClient>();
builder.Services.AddScoped<ICarsRepo, CarsRepo>();
builder.Services.AddMemoryCache();
//builder.Services.AddScoped<ICache<IEnumerable<Car>>, InMemoryCache<IEnumerable<Car>>>();
builder.Services.AddScoped<ICache<IEnumerable<Car>>, RedisCache<IEnumerable<Car>>>();
builder.Services.AddScoped<IDataSyncService, DataSyncService>();
builder.Services.AddHostedService<DataImport>();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["Redis:ConnectionString"];
    options.InstanceName = builder.Configuration["Redis:Prefix"];
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
