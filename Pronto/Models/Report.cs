using System;
using System.Text.Json.Serialization;

namespace Pronto.Models
{
    public class Report
    {
        [JsonIgnore] public int ReportId { get; set; }
        public int UserId { get; set; }
        public int DeviceId { get; set; }
        public int BusinessId { get; set; }
        public string? ErrorCode {  get; set; }
        public string Description { get; set; }
        public int Severity { get; set; } //from 1 to 4
        public DateTime CreatedAt { get; set; }
        public string? AttachmentUrl { get; set; }

    }
}