using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using YBS2.Data.Enums;
using YBS2.TypeConverter;

namespace YBS2.Service.Dtos.Inputs
{
    public class TourInputDto
    {
        public Guid? YachtId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public List<IFormFile> ImageURLs { get; set; }
        [Required]
        public int Priority { get; set; }
        [Required]
        public string Location { get; set; }
        [Required]
        [JsonConverter(typeof(JSonConverterTimeSpan))]
        public TimeSpan StartTime { get; set; }
        [Required]
        [JsonConverter(typeof(JSonConverterTimeSpan))]
        public TimeSpan EndTime { get; set; }
        public int? Duration { get; set; }
        public string? DurationUnit { get; set; }
        [Required]
        public EnumTourType Type { get; set; }
        [Required]
        public string Description { get; set; }
        public List<Guid> Docks { get; set; }
    }
}