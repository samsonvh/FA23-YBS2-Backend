using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YBS2.Data.Enums;

namespace YBS2.Service.Dtos.PageRequests
{
    public class MemberPageRequest :DefaultPageRequest
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public EnumMemberStatus Status { get; set; }
    }
}