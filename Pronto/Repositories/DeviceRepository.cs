using Pronto.Models;
using Pronto.Data;
using Dapper;
using Pronto.Repositories.Interfaces;
using Pronto.DTOs;

namespace Pronto.Repositories
{
    public class DeviceRepository : IDeviceRepository
    { 
        private readonly DatabaseHelper _databaseHelper;

        public DeviceRepository(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public async Task<ApiResponse<Device>> GetDeviceByIdAsync(int deviceId)
        {
            using var connection = _databaseHelper.CreateConnection();
            var sql = @"SELECT DeviceId, BusinessId, Make, Model, SerialNumber FROM device WHERE DeviceId = @DeviceId";
            var device = await connection.QuerySingleOrDefaultAsync<Device>(sql, new { DeviceId = deviceId });
            
            if (device == null)
            {
                return new ApiResponse<Device>
                {
                    Success = false,
                    ErrorMessage = "Device not found.",
                    StatusCode = 404
                };
            }

            return new ApiResponse<Device>
            {
                Success = true,
                Data = device,
                StatusCode = 200
            };
        }

        public async Task<ApiResponse<Device>> CreateDeviceAsync(Device device)
        {
            using var connection = _databaseHelper.CreateConnection();
            var sql = @"INSERT INTO device (BusinessId, Make, Model, SerialNumber)
                    VALUES (@BusinessId, @Make, @Model, @SerialNumber);
                    SELECT LAST_INSERT_ID();";

            var deviceId =  await connection.ExecuteScalarAsync<int>(sql, device);

            if (deviceId == 0)
            {
                return new ApiResponse<Device>
                {
                    Success = false,
                    ErrorMessage = "Error creating device.",
                    StatusCode = 500
                };
            }

            device.DeviceId = deviceId;

            return new ApiResponse<Device>
            {
                Success = true,
                Data = device,
                StatusCode = 200
            };
        }

        public async Task<ApiResponse<Device>> UpdateDeviceAsync(int deviceId, DeviceUpdateDTO updatedDeviceDTO)
        {
            using var connection = _databaseHelper.CreateConnection();
            var deviceExists = await connection.QuerySingleOrDefaultAsync<Device>("SELECT * FROM device WHERE deviceId=@DeviceId", new { DeviceId = deviceId });

            if (deviceExists == null)
            {
                return new ApiResponse<Device>
                {
                    Success = false,
                    ErrorMessage = "Device not found.",
                    StatusCode = 404
                };
            }

            deviceExists.BusinessId = updatedDeviceDTO.BusinessId ?? deviceExists.BusinessId;
            deviceExists.Make = updatedDeviceDTO.Make ?? deviceExists.Make;
            deviceExists.Model = updatedDeviceDTO.Model ?? deviceExists.Model;
            deviceExists.SerialNumber = updatedDeviceDTO.SerialNumber ?? deviceExists.SerialNumber;

            var sql = @"UPDATE device SET BusinessId = @BusinessId, Make = @Make, Model = @Model, SerialNumber = @SerialNumber WHERE DeviceId = @DeviceId  AND (BusinessId != @BusinessId OR Make != @Make OR Model != @Model OR SerialNumber != @SerialNumber)";

            var rowsAffected = await connection.ExecuteAsync(sql, deviceExists);

            if (rowsAffected == 0)
            {
                return new ApiResponse<Device>
                {
                    Success = false,
                    ErrorMessage = "No changes were made. The data was already up-to-date.",
                    StatusCode = 200
                };
            }

            return new ApiResponse<Device>
            {
                Success = true,
                Data = deviceExists,
                StatusCode = 200
            };
        }

        public async Task<ApiResponse<IEnumerable<Device>>> GetDevicesByBusinessIdAsync(int businessId)
        {
            using var connection = _databaseHelper.CreateConnection();
            var sql = "SELECT * FROM device WHERE businessId = @businessId";

            var devices = (await connection.QueryAsync<Device>(sql, new { BusinessId = businessId })).ToList();

            if (devices == null || !devices.Any())
            {
                return new ApiResponse<IEnumerable<Device>>
                {
                    Success = false,
                    ErrorMessage = "No devices found for the given business ID.",
                    StatusCode = 404
                };
            }

            return new ApiResponse<IEnumerable<Device>>
            {
                Success = true,
                Data = devices,
                StatusCode = 200
            };
        }
    }
}