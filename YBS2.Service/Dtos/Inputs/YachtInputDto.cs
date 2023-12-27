using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YBS2.Data.Enums;

namespace YBS2.Service.Dtos.Inputs
{
    public class YachtInputDto
    {
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public string LOA { get; set; }
        public string BEAM { get; set; }
        public string DRAFT { get; set; }
        public int TotalCrew { get; set; }
        public int TotalPassenger { get; set; }
        public int Cabin { get; set; }
        public int Type { get; set; }
        public EnumYachtStatus Status { get; set; }
    }
}