using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YBS2.Data.Enums;

namespace YBS2.Service.Dtos.PageRequests
{
    public class TourPageRequest : DefaultPageRequest
    {
        public string? Name { get; set; }
        public float MinPrice { get; set; }
        public float MaxPrice { get; set; }
        public string? Location { get; set; }
        public int MinGuest { get; set; }
        public int MaxGuest { get; set; }
        public EnumTourType? Type { get; set; }
        public EnumTourStatus? Status { get; set; }
    }
}