using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS2.Service.Dtos.Details
{
    public class TourDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string ImageURL { get; set; }
        public int Priority { get; set; }
        public string Location { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int Duration { get; set; }
        public string DurationUnit { get; set; }
        public int MaximumGuest { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}