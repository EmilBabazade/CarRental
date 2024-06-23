using AutoMapper;
using Clients.SouthRentals;
using Domain.RentalCar;

namespace Clients.MappingProfiles;

public class SouthRentalsResponseMappingProfile : Profile
{
    public SouthRentalsResponseMappingProfile()
    {
        CreateMap<SouthRentalsResponse, Car>().ReverseMap();
    }
}