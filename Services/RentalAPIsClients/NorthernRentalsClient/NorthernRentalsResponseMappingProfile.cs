using AutoMapper;
using Domain.RentalCar;

namespace Services.RentalAPIsClients.NorthernRentalsClient;

public class NorthernRentalsResponseMappingProfile : Profile
{
    public NorthernRentalsResponseMappingProfile()
    {
        CreateMap<NorthernRentalsResponse, Car>()
            .ForMember(c => c.Cost, cd => cd.MapFrom(r => r.Price))
            .ForMember(c => c.QuoteNumber, cd => cd.MapFrom(r => r.Id))
            .ForMember(c => c.Sipp, cd => cd.MapFrom(r => r.SippCode))
            .ForMember(c => c.Currency, cd => cd.MapFrom(r => r.Currency))
            .ForMember(c => c.ImageURL, cd => cd.MapFrom(r => r.Image))
            .ForMember(c => c.LogoURL, cd => cd.MapFrom(r => r.SupplierLogo))
            .ForMember(c => c.Vehicle, cd => cd.MapFrom(r => r.VehicleName))
            .ReverseMap();
    }
}
