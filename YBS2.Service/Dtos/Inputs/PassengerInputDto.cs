using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YBS2.Data.Enums;

namespace YBS2.Service.Dtos.Inputs
{
    public class PassengerInputDto
    {
        public string FullName { get; set; }
        public DateTime DOB { get; set; }
        public EnumGender Gender { get; set; }
        public string IdentityNumber { get; set; }
    }
}