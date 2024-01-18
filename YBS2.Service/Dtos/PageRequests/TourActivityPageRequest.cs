using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS2.Service.Dtos.PageRequests
{
    public class TourActivityPageRequest : DefaultPageRequest
    {
        public string? Name { get; set; }
        public string? Location { get; set; }
    }
}