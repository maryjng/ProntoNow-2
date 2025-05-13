using Pronto.Models;
using Pronto.DTOs;

namespace Pronto.Repositories.Interfaces
{
    public interface IReportRepository
    {
        Task<ApiResponse<Report>> CreateReportAsync(Report report);
        Task<ApiResponse<Report>> GetReportByIdAsync(int reportId);
        Task<ApiResponse<Report>> UpdateReportAsync(int reportId, ReportUpdateDTO reportUserDTO);
        Task<ApiResponse<IEnumerable<Report>>> GetReportsByDeviceIdAsync(int deviceId);
    }
}
