namespace Domain.RentalCar;

public interface ICar
{
    decimal Cost { get; set; }
    string Currency { get; set; }
    string ImageURL { get; set; }
    string LogoURL { get; set; }
    string QuoteNumber { get; set; }
    string Sipp { get; set; }
    string Vehicle { get; set; }
}