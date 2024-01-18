using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS2.Service.Dtos.Listings
{
    public class TourActivityListingDto
    {
        public Guid Id { get; set; }
        public Guid TourId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Status { get; set; }
    }
}