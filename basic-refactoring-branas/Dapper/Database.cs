using Dapper;
using System.Data;
using System.Data.Common;

namespace basic_refactoring_branas.Dapper
{
    public class Database : IDatabase
    {
        private readonly IDbConnection _connection;

        public Database(IDbConnection connection)
        {
            _connection = connection;
        }

        public IDbConnection Connection => _connection;

        public Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null)
        {
            return _connection.QueryFirstOrDefaultAsync<T>(sql, param);
        }

        public Task<int> ExecuteAsync(string sql, object param = null)
        {
            return _connection.ExecuteAsync(sql, param);
        }
    }
}
