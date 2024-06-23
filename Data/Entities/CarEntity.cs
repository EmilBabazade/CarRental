using Domain.RentalCar;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;
public class CarEntity : IUnique, ICar
{
    [Key]
    public int Id { get; set; }
    public decimal Cost { get; set; }
    public string Currency { get; set; }
    public string ImageURL { get; set; }
    public string LogoURL { get; set; }
    public string QuoteNumber { get; set; }
    public string Sipp { get; set; }
    public string Vehicle { get; set; }
}
