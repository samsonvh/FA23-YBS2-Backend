using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS2.Service.Dtos.Details
{
    public class MembershipRegistrationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public float? DiscountPercent { get; set; }
        public DateTime? MembershipStartDate { get; set; }
        public DateTime? MembershipExpireDate { get; set; }
        public string? DeviceToken { get; set; }
        public string Status { get; set; }
    }
}