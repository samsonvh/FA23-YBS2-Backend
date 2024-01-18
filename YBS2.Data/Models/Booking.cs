using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using YBS2.Data.Enums;

namespace YBS2.Data.Models
{
    public class Booking
    {
        public Guid Id { get; set; }
        public Guid? MemberId { get; set; }
        [ForeignKey("MemberId")]
        public Member? Member { get; set; }
        public Guid TourId { get; set; }
        [ForeignKey("TourId")]
        public Tour Tour { get; set; }
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
        public EnumBookingType Type { get; set; }
        public EnumBookingStatus Status { get; set; }
        public EnumPaymentMethod PaymentMethod { get; set; }
        public EnumPaymentStatus PaymentStatus { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow.AddHours(7);
        public string DeviceToken { get; set; }
        public ICollection<Passenger> Passengers { get; set; }
        public ICollection<Transaction>? Transactions { get; set; }
        public ICollection<BookingActivity>? BookingActivities { get; set; }
    }
}