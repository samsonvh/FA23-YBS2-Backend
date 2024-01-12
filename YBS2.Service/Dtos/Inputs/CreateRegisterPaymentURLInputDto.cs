using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS2.Service.Dtos.Inputs
{
    public class CreateRegisterPaymentURLInputDto
    {
        public Guid MembershipRegistrationId { get; set; }
        public Guid MembershipPackageId { get; set; }
    }
}