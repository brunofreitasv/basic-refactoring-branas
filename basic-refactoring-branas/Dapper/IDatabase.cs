using System.Data;

namespace basic_refactoring_branas.Dapper
{
    public interface IDatabase
    {
        IDbConnection Connection { get; }
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null);
        Task<int> ExecuteAsync(string sql, object param = null);
    }
}
