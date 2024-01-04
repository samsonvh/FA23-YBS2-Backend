using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using YBS2.Data.Enums;

namespace YBS2.Service.Dtos.Inputs
{
    public class DockInputDto
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public List<IFormFile> ImageURLs { get; set; }
        public string Description { get; set; }
        public EnumDockStatus Status { get; set; }
    }
}