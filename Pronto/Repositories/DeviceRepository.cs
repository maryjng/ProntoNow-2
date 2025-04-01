using Pronto.Models;
using Pronto.Data;
using Dapper;

namespace Pronto.Repositories
{
    public class DeviceRepository 
    { 
        private readonly DatabaseHelper _databaseHelper;

        public DeviceRepository(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public async Task<Device> GetDeviceByIdAsync(int deviceId)
        {
            using var connection = _databaseHelper.CreateConnection();
            var sql = @"SELECT device_id as DeviceId, business_id as BusinessId, make as Make, model as Model, serial_number as SerialNumber FROM device WHERE device_id = @DeviceId";
            var device = await connection.QuerySingleOrDefaultAsync<Device>(sql, new { DeviceId = deviceId });
            return device;
        }

        public async Task<int> CreateDeviceAsync(Device device)
        {
            using var connection = _databaseHelper.CreateConnection();
            var sql = @"INSERT INTO device (device_id, business_id, make, model, serial_number)
                    VALUES (@DeviceId, @BusinessId, @Make, @Model, @SerialNumber);
                    SELECT LAST_INSERT_ID();";

            return await connection.ExecuteScalarAsync<int>(sql, device);    
        }

    }
}