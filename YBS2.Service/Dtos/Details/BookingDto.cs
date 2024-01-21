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
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string SpecialRequest { get; set; }
        public Guid TourId { get; set; }
        public DateTime BookingDate { get; set; }
        public float TotalAmount { get; set; }
        public float? Point { get; set; }
        public string TourName { get; set; }
        public string Location { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public int Duration { get; set; }
        public EnumTimeUnit DurationUnit { get; set; }
        public EnumTourType TourType { get; set; }
        public int TotalPassengers { get; set; }
        public string Note { get; set; }
        public bool isIncludeBooker { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public string DeviceToken { get; set; }
    }
}