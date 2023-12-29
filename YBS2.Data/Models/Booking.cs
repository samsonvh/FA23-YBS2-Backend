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
        public int TotalPassengers { get; set; }
        public string Note { get; set; }
        public EnumBookingStatus Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}