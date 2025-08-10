using System.ComponentModel.DataAnnotations;

namespace IDesign.Host.Models
{
    public class CityDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public int CountryId { get; set; }
    }
}
