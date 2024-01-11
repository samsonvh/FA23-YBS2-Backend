using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS2.Service.Dtos.Details
{
    public class CreateMemberDto
    {
        public Guid membershipRegistrationId { get; set; }
        public string paymentURL { get; set; }
    }
}