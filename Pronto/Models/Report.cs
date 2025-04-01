using System;

namespace Pronto.Models
{
    public class Report
    {
        public int ReportId { get; set; }
        public int UserId { get; set; }
        public int BusinessId { get; set; }
        public int DeviceId { get; set; }
        public int? ErrorCode {  get; set; }
        public string Description { get; set; }
        public int Severity { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}