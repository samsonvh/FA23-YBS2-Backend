using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YBS2.Service.Dtos.Listings
{
    public class PassengerListingDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
    }
}