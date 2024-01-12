using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS2.Service.Dtos
{
    public class YachtDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string[] ImageURL { get; set; }
        public string Description { get; set; }
        public string Manufacturer { get; set; }
        public string LOA { get; set; }
        public string BEAM { get; set; }
        public string DRAFT { get; set; }
        public int TotalCrew { get; set; }
        public int TotalPassengers { get; set; }
        public int Cabin { get; set; }
        public string Status { get; set; }
    }
}