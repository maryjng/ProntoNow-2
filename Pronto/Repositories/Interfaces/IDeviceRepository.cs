using Pronto.Models;
using Pronto.DTOs;

namespace Pronto.Repositories.Interfaces
{
    public interface IDeviceRepository
    {
        Task<ApiResponse<Device>> CreateDeviceAsync(Device device);
        Task<ApiResponse<Device>> GetDeviceByIdAsync(int deviceId);
        Task<ApiResponse<Device>> UpdateDeviceAsync(int deviceId, DeviceUpdateDTO updatedDeviceDTO);
        Task<ApiResponse<IEnumerable<Device>>> GetDevicesByUserIdAsync(int userId);

    }
}
