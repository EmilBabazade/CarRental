using AutoMapper;
using Clients.BestRentals;
using Domain.RentalCar;

namespace Clients.MappingProfiles;
public class BestRentalsResponseMappingProfile : Profile
{
    public BestRentalsResponseMappingProfile()
    {
        CreateMap<BestRentalsResponse, Car>().ReverseMap();
    }
}
