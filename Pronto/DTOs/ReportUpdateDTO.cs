namespace Pronto.DTOs
{
    public class ReportUpdateDTO
    {
        public int? DeviceId { get; set; }
        public int? BusinessId { get; set; }
        public string? ErrorCode { get; set; }
        public string? Description { get; set; }
        public int? Severity { get; set; }
        public string? AttachmentUrl { get; set; }

    }
}
