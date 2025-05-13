using Pronto.Models;
using Pronto.Data;
using Dapper;
using Pronto.Repositories.Interfaces;
using Pronto.DTOs;
using System.Data;

namespace Pronto.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly IDatabaseHelper _databaseHelper;

        public ReportRepository(IDatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public virtual async Task<T?> QuerySingleOrDefaultAsync<T>(IDbConnection connection, string sql, object? param = null) =>
    await connection.QuerySingleOrDefaultAsync<T>(sql, param);

        public async Task<ApiResponse<Report>> GetReportByIdAsync(int reportId)
        {
            using var connection = _databaseHelper.CreateConnection();
            var sql = "SELECT * FROM report WHERE ReportId = @ReportId";

            var report = await QuerySingleOrDefaultAsync<Report>(connection, sql, new { ReportId = reportId });

            if (report == null)
            {
                return new ApiResponse<Report>
                {
                    Success = false,
                    ErrorMessage = "Report not found.",
                    StatusCode = 404
                };
            }

            return new ApiResponse<Report>
            {
                Success = true,
                Data = report,
                StatusCode = 200
            };
        }

        public async Task<ApiResponse<Report>> CreateReportAsync(Report report)
        {
            using var connection = _databaseHelper.CreateConnection();
            var sql = @"INSERT INTO report (UserId, BusinessId, DeviceId, ErrorCode, Description, Severity, AttachmentUrl) 
                        VALUES (@UserId, @BusinessId, @DeviceId, @ErrorCode, @Description, @Severity, @AttachmentUrl); 
                        SELECT LAST_INSERT_ID();";

            var reportId = await connection.ExecuteScalarAsync<int>(sql, report);

            if (reportId == 0)
            {
                return new ApiResponse<Report>
                {
                    Success = false,
                    ErrorMessage = "Error creating report.",
                    StatusCode = 500
                };
            }

            //report.ReportId = reportId;
            return new ApiResponse<Report>
            {
                Success = true,
                Data = report,
                StatusCode = 201
            };
        }

        public async Task<ApiResponse<Report>> UpdateReportAsync(int reportId, ReportUpdateDTO updatedReportDTO)
        {
            using var connection = _databaseHelper.CreateConnection();
            var reportExists = await connection.QuerySingleOrDefaultAsync<Report>("SELECT * FROM report WHERE ReportId=@ReportId", new { ReportId = reportId });

            if (reportExists == null)
            {
                return new ApiResponse<Report>
                {
                    Success = false,
                    ErrorMessage = "Report not found.",
                    StatusCode = 404
                };
            }

            reportExists.BusinessId = updatedReportDTO.BusinessId ?? reportExists.BusinessId;
            reportExists.DeviceId = updatedReportDTO.DeviceId ?? reportExists.DeviceId;
            reportExists.ErrorCode = updatedReportDTO.ErrorCode ?? reportExists.ErrorCode;
            reportExists.Description = updatedReportDTO.Description ?? reportExists.Description; 
            reportExists.Severity = updatedReportDTO.Severity ?? reportExists.Severity;
            reportExists.AttachmentUrl = updatedReportDTO.AttachmentUrl ?? reportExists.AttachmentUrl;

            var sql = @"UPDATE report SET BusinessId = @BusinessId, DeviceId = @DeviceId, ErrorCode = @ErrorCode, Description = @Description, Severity = @Severity, AttachmentUrl = @AttachmentUrl WHERE ReportId = @ReportId AND (BusinessId != @BusinessId OR DeviceId != @DeviceId OR ErrorCode != @ErrorCode OR Description != @Description OR Severity != @Severity OR AttachmentUrl != @AttachmentUrl)";

            var rowsAffected = await connection.ExecuteAsync(sql, reportExists);

            if (rowsAffected == 0)
            {
                return new ApiResponse<Report>
                {
                    Success = false,
                    ErrorMessage = "No changes were made. The data was already up-to-date.",
                    StatusCode = 200
                };
            }

            return new ApiResponse<Report>
            {
                Success = true,
                Data = reportExists,
                StatusCode = 200
            };
        }

        public async Task<ApiResponse<IEnumerable<Report>>> GetReportsByDeviceIdAsync(int deviceId)
        {
            using var connection = _databaseHelper.CreateConnection();
            var sql = "SELECT * FROM report WHERE DeviceId = @DeviceId";

            var reports = (await connection.QueryAsync<Report>(sql, new { DeviceId = deviceId })).ToList();

            if (reports == null || !reports.Any())
            {
                return new ApiResponse<IEnumerable<Report>>
                {
                    Success = false,
                    ErrorMessage = "No reports found for the given device ID.",
                    StatusCode = 404
                };
            }

            return new ApiResponse<IEnumerable<Report>>
            {
                Success = true,
                Data = reports,
                StatusCode = 200
            };
        }

        public async Task<ApiResponse<IEnumerable<Report>>> GetReportsByUserIdAsync(int userId)
        {
            using var connection = _databaseHelper.CreateConnection();
            var sql = "SELECT * FROM report WHERE userId = @userId";

            var reports = (await connection.QueryAsync<Report>(sql, new { UserId = userId })).ToList();

            if (reports == null || !reports.Any())
            {
                return new ApiResponse<IEnumerable<Report>>
                {
                    Success = false,
                    ErrorMessage = "No reports found for the given user ID.",
                    StatusCode = 404
                };
            }

            return new ApiResponse<IEnumerable<Report>>
            {
                Success = true,
                Data = reports,
                StatusCode = 200
            };
        }
    }
}
