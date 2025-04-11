using System.Data;
using System.Threading.Tasks;

namespace Pronto.Data
{
    public interface IDatabaseHelper
    {
        IDbConnection CreateConnection();
    }
}