using AutoMapper;
using Domain.RentalCar;

namespace Services.RentalAPIsClients.SouthRentals;

public class SouthRentalsResponseMappingProfile : Profile
{
    public SouthRentalsResponseMappingProfile()
    {
        CreateMap<SouthRentalsResponse, Car>()
            .ForMember(c => c.Cost, cd => cd.MapFrom(r => r.Price))
            .ForMember(c => c.QuoteNumber, cd => cd.MapFrom(r => r.QuoteNumber))
            .ForMember(c => c.Sipp, cd => cd.MapFrom(r => r.AcrissCode))
            .ForMember(c => c.Currency, cd => cd.MapFrom(r => r.Currency))
            .ForMember(c => c.ImageURL, cd => cd.MapFrom(r => r.ImageLink))
            .ForMember(c => c.LogoURL, cd => cd.MapFrom(r => r.LogoLink))
            .ForMember(c => c.Vehicle, cd => cd.MapFrom(r => r.VehicleName))
            .ReverseMap();
    }
}