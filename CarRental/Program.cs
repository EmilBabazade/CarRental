using Clients.BestRentals;
using Clients.MappingProfiles;
using Clients.NorthernRentalsClient;
using Clients.SouthRentals;
using Data;
using Data.MappingProfiles;
using Data.Repos.Cars;
using Microsoft.EntityFrameworkCore;

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
builder.Services.AddHttpClient<ISouthRentalsClient, SouthRentalsClient>();
builder.Services.AddHttpClient<INorthernRentalsClient, NorthernRentalsClient>();
builder.Services.AddHttpClient<IBestRentalsClient, BestRentalsClient>();
builder.Services.AddScoped<ICarsRepo, CarsRepo>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
