using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS2.Service.Dtos.Listings
{
    public class BookingListingDto
    {
        public Guid Id { get; set; }
        public DateTime BookingDate { get; set; }
        public int TotalPassengers { get; set; }
        public string Status { get; set; }
    }
}