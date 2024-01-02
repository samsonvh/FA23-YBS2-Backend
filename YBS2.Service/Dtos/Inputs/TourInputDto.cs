using System;
using System.Collections.Generic;
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
        public string Name { get; set; }
        public float Price { get; set; }
        public List<IFormFile> ImageURL { get; set; }
        public int Priority { get; set; }
        public string Location { get; set; }
        [JsonConverter(typeof(JSonConverterTimeSpan))]
        public TimeSpan StartTime { get; set; }
        [JsonConverter(typeof(JSonConverterTimeSpan))]
        public TimeSpan EndTime { get; set; }
        public int? Duration { get; set; }
        public string? DurationUnit { get; set; }
        public EnumTourType Type { get; set; }
        public string Description { get; set; }
    }
}