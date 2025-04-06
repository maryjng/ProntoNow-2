using Pronto.Models;
using System.Threading.Tasks;

namespace Pronto.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<int> CreateUserAsync(User user);
        Task<User> GetUserByIdAsync(string email);

    }
}
