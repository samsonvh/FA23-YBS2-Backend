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
        public int TotalCrew { get; set; }
        public int Cabin { get; set; }
        public int Type { get; set; }
        public string Status { get; set; }
    }
}