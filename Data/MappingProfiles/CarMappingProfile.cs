using AutoMapper;
using Data.Entities;
using Domain.RentalCar;

namespace Data.MappingProfiles;
public class CarMappingProfile : Profile
{
    public CarMappingProfile()
    {
        CreateMap<CarEntity, Car>().ReverseMap();
    }
}
