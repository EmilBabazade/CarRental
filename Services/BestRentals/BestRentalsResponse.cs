namespace Services.BestRentals;

public class BestRentalsResponse
{
    public string UniqueId { get; set; }
    public decimal RentalCost { get; set; }
    public string RentalCostCurrency { get; set; }
    public string Vehicle { get; set; }
    public string Sipp { get; set; }
    public string ImageLink { get; set; }
    public string Logo { get; set; }
}