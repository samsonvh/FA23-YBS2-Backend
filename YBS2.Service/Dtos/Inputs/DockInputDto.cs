using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using YBS2.Data.Enums;

namespace YBS2.Service.Dtos.Inputs
{
    public class DockInputDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public List<IFormFile> ImageURLs { get; set; }
        [Required]
        public string Description { get; set; }
    }
}