using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YBS2.Data.Enums;

namespace YBS2.Service.Dtos
{
    public class BookingDto
    {
        public int Id { get; set; }
        public DateTime BookingDate { get; set; }
        public int TotalPassengers { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}