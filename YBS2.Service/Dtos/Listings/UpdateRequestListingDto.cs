using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YBS2.Service.Dtos.Listings
{
    public class UpdateRequestListingDto
    {
        public Guid Id { get; set; }
        public string LogoURL { get; set; }
        public string CompanyName { get; set; }
        public string ApproverUserName { get; set; }
    }
}
