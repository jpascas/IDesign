using System.ComponentModel.DataAnnotations;

namespace IDesign.Access.Entities;

public class City
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = string.Empty;
    [Required]
    public int CountryId { get; set; }
    public Country? Country { get; set; }
}
