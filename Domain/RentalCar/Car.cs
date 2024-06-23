namespace Domain.RentalCar;

public class Car : ICar
{
    public string QuoteNumber { get; set; }
    public decimal Cost { get; set; }
    public string Currency { get; set; }
    public string Vehicle { get; set; }
    public string Sipp { get; set; }
    public string ImageURL { get; set; }
    public string LogoURL { get; set; }
}
