using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS2.Service.Dtos.Listings
{
    public class YachtListingDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public string Manufacturer { get; set; }
        public int TotalCrews { get; set; }
        public int TotalPassengers { get; set; }
        public int Cabin { get; set; }
        public string Status { get; set; }
        public string Type { get; set; }
    }
}