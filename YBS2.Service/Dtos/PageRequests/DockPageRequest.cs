using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YBS2.Data.Enums;

namespace YBS2.Service.Dtos.PageRequests
{
    public class DockPageRequest : DefaultPageRequest
    {
        public string? Name { get; set; }
        public EnumDockStatus? Status { get; set; }
    }
}