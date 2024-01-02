using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using YBS2.Data.Enums;

namespace YBS2.Service.Dtos.PageRequests
{
    public class BookingPageRequest : DefaultPageRequest
    {
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public EnumBookingStatus? Status { get; set; }
    }
}