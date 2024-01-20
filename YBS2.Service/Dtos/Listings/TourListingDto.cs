using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS2.Service.Dtos.Listings
{
    public class TourListingDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; }
        public float Price { get; set; }
        public float? DiscountPrice { get; set; }
        public int Duration { get; set; }
        public int DurationUnit { get; set; }
        public int Priority { get; set; }
        public string Location { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
    }
}