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
            var sql = @"SELECT DeviceId, BusinessId, Make, Model, SerialNumber FROM device WHERE device_id = @DeviceId";
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
            var sql = @"INSERT INTO device (device_id, business_id, make, model, serial_number)
                    VALUES (@DeviceId, @BusinessId, @Make, @Model, @SerialNumber);
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

        public Task<ApiResponse<Device>> UpdateDeviceAsync(int deviceId, DeviceUpdateDTO updatedDeviceDTO)
        {
            throw new NotImplementedException();
        }
    }
}