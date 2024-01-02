using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YBS2.Data.Enums;

namespace YBS2.Service.Dtos.Inputs
{
    public class TourInputDto
    {
        public Guid Id { get; set; }
        public Guid? YachtId { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string ImageURL { get; set; }
        public int Priority { get; set; }
        public string Location { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int Duration { get; set; }
        public string DurationUnit { get; set; }
        public EnumTourType Type { get; set; }
        public string Description { get; set; }
    }
}