using Pronto.Models;
using Pronto.DTOs;
using System.Threading.Tasks;

namespace Pronto.Repositories.Interfaces
{
    public interface IDeviceRepository
    {
        Task<ApiResponse<Device>> CreateDeviceAsync(Device device);
        Task<ApiResponse<Device>> GetDeviceByIdAsync(int deviceId);
        Task<ApiResponse<Device>> UpdateDeviceAsync(int deviceId, DeviceUpdateDTO updatedDeviceDTO);
    }
}
