using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using YBS2.Data.Enums;

namespace YBS2.Service.Dtos.Inputs
{
    public class BookingInputDto
    {
        public Guid TourId { get; set; }
        public DateTime BookingDate { get; set; }
        public string? Note { get; set; }
        public float TotalAmount { get; set; }
        public EnumBookingType Type { get; set; }
        public List<PassengerInputDto>? Passengers { get; set; }
        public bool isIncludeBooker { get; set; } = true;
    }
}
