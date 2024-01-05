using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YBS2.Data.Enums;

namespace YBS2.Service.Dtos.Details
{
    public class BookingDto
    {
        public Guid Id { get; set; }
        public DateTime BookingDate { get; set; }
        public float TotalAmount { get; set; }
        public int TotalPassengers { get; set; }
        public string Note { get; set; }
        public bool isIncludeBooker { get; set; }
        public EnumBookingType Type { get; set; }
        public EnumBookingStatus Status { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow.AddHours(7);
    }
}