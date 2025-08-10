﻿using System.ComponentModel.DataAnnotations;

namespace IDesign.Manager.Models
{
    public class CountryDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public List<CityDto> Cities { get; set; } = new();
    }
}
