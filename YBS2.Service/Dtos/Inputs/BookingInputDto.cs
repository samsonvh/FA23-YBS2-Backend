using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using YBS2.Data.Enums;

namespace YBS2.Service.Dtos.Inputs
{
    public class BookingInputDto
    {
        public int Id { get; set; }
        public int? MemberId { get; set; }
        public int TourId { get; set; }
        public DateTime BookingDate { get; set; }
        public int TotalPassengers { get; set; }
        public string Note { get; set; }
        public EnumBookingStatus Status { get; set; }
    }
}
