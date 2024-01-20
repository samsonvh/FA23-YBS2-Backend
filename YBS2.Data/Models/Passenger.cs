using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using YBS2.Data.Enums;

namespace YBS2.Data.Models
{
    public class Passenger
    {
        public Guid Id { get; set; }
        public Guid BookingId { get; set; }
        [ForeignKey("BookingId")]
        public Booking Booking { get; set; }
        public string FullName { get; set; }
        public DateTime DOB { get; set; }
        public EnumGender Gender { get; set; }
        public string IdentityNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string SpecialRequest { get; set; }
        public bool IsLeader { get; set; }
    }
}