using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using YBS2.Data.Enums;

namespace YBS2.Service.Dtos.Inputs
{
    public class YachtInputDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string ImageURL { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Manufacturer { get; set; }
        [Required]
        public string LOA { get; set; }
        [Required]
        public string BEAM { get; set; }
        [Required]
        public string DRAFT { get; set; }
        [Required]
        public int TotalCrew { get; set; }
        [Required]
        public int TotalPassenger { get; set; }
        [Required]
        public int Cabin { get; set; }
        [Required]
        public EnumYachtStatus Status { get; set; }
    }
}