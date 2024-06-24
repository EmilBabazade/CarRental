using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

internal interface IUnique
{
    [Key]
    public int Id { get; set; }
}
