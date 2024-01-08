using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS2.Service.Dtos.PageRequests
{
    public class YachtPageRequest : DefaultPageRequest
    {
        public string? Name { get; set; }
        public int? MinPassengers { get; set; }
        public int? MaxPassengers { get; set; }
        public int? MinCrew { get; set; }
        public int? MaxCrew { get; set; }
        public int? MinCabin { get; set; }
        public int? MaxCabin { get; set; }
    }
}