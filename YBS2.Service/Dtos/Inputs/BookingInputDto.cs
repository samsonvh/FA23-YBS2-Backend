using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using YBS2.Data.Enums;

namespace YBS2.Service.Dtos.Inputs
{
    public class BookingInputDto
    {
        [Required]
        public Guid TourId { get; set; }
        [Required]
        public DateTime BookingDate { get; set; }
        public string? Note { get; set; }
        [Required]
        public EnumBookingType Type { get; set; }
        public List<PassengerInputDto>? Passengers { get; set; }
        [Required]
        public bool isIncludeBooker { get; set; } = true;
    }
}
