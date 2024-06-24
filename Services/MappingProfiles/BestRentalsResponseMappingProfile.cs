using AutoMapper;
using Clients.BestRentals;
using Domain.RentalCar;

namespace Clients.MappingProfiles;
public class BestRentalsResponseMappingProfile : Profile
{
    public BestRentalsResponseMappingProfile()
    {
        CreateMap<BestRentalsResponse, Car>()
            .ForMember(c => c.Cost, cd => cd.MapFrom(r => r.RentalCost))
            .ForMember(c => c.QuoteNumber, cd => cd.MapFrom(r => r.UniqueId))
            .ForMember(c => c.Sipp, cd => cd.MapFrom(r => r.Sipp))
            .ForMember(c => c.Currency, cd => cd.MapFrom(r => r.RentalCostCurrency))
            .ForMember(c => c.ImageURL, cd => cd.MapFrom(r => r.ImageLink))
            .ForMember(c => c.LogoURL, cd => cd.MapFrom(r => r.Logo))
            .ForMember(c => c.Vehicle, cd => cd.MapFrom(r => r.Vehicle))
            .ReverseMap();
    }
}
