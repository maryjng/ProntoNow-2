using System;
using System.Text.Json.Serialization;

namespace Pronto.Models
{
    public class Device
    {
        [JsonIgnore] public int DeviceId { get; set; }
        public int BusinessId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
    }
}