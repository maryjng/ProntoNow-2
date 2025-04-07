using System.Data;
using System.Threading.Tasks;

namespace Pronto.Data
{
    public interface IDatabaseConnection
    {
        // Method that mimics the Dapper `QuerySingleOrDefaultAsync` method
        Task<T> QuerySingleOrDefaultAsync<T>(
            string sql,
            object param = null,
            IDbTransaction transaction = null,
            int? commandTimeout = null,
            CommandType? commandType = null);
    }
}