using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using YBS2.Data.Enums;

namespace YBS2.Service.Dtos.Inputs
{
    public class PassengerInputDto
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public DateTime DOB { get; set; }
        [Required]
        public EnumGender Gender { get; set; }
        [Required]
        public string IdentityNumber { get; set; }
    }
}