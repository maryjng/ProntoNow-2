using Pronto.Models;
using Pronto.DTOs;
using System.Threading.Tasks;

namespace Pronto.Repositories.Interfaces
{
    public interface IBusinessRepository
    {
        Task<ApiResponse<Business>> CreateBusinessAsync(Business business);
        Task<ApiResponse<Business>> GetBusinessByIdAsync(int businessId);
        Task<ApiResponse<Business>> UpdateBusinessAsync(int businessId, BusinessUpdateDTO businessUpdateDTO);
    }
}
