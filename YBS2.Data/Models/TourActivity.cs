using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using YBS2.Data.Enums;

namespace YBS2.Data.Models
{
    public class TourActivity
    {
        public Guid Id { get; set; }
        public Guid TourId { get; set; }
        [ForeignKey("TourId")]
        public Tour Tour { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string Description { get; set; }
        public EnumActivityStatus Status { get; set; }
    }
}