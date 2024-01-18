using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using YBS2.TypeConverter;

namespace YBS2.Service.Dtos.Inputs
{
    public class TourActivityInputDto
    {   
        
        [Required]
        public string Name { get; set; }
        public string? Location { get; set; }
        [Required]  
        [JsonConverter(typeof(JSonConverterTimeSpan))]
        public TimeSpan StartTime { get; set; }
        [Required]
        [JsonConverter(typeof(JSonConverterTimeSpan))]
        public TimeSpan EndTime { get; set; }
        public string? Description { get; set; }
    }
}