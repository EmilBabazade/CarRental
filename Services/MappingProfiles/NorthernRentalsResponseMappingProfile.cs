using AutoMapper;
using Clients.NorthernRentalsClient;
using Domain.RentalCar;

namespace Clients.MappingProfiles;

public class NorthernRentalsResponseMappingProfile : Profile
{
    public NorthernRentalsResponseMappingProfile()
    {
        CreateMap<NorthernRentalsResponse, Car>().ReverseMap();
    }
}
