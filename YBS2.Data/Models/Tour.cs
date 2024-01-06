using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using YBS2.Data.Enums;

namespace YBS2.Data.Models
{
    public class Tour
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public Company Company { get; set; }
        public Guid? YachtId { get; set; }
        [ForeignKey("YachtId")]
        public Yacht? Yacht { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public string ImageURL { get; set; }
        public int Priority { get; set; }
        public string Location { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int Duration { get; set; }
        public EnumTimeUnit DurationUnit { get; set; }
        public int MaximumGuest { get; set; }
        public EnumTourType Type { get; set; }
        public string Description { get; set; }
        public EnumTourStatus Status { get; set; }
        public ICollection<Booking>? Bookings { get; set; }
        public ICollection<TourDock> TourDocks { get; set; }
    }
}