using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace EmployeeManagement.DapperContent
{
    public class DapperContent : IDapperContent
    {
        private readonly IDbConnection _dbConnection;
        public DapperContent(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text)
        {
            using IDbConnection db = new SqlConnection(_dbConnection.ConnectionString);
            return db.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();
        }

        public List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using IDbConnection db = new SqlConnection(_dbConnection.ConnectionString);
            return db.Query<T>(sp, parms, commandType: commandType).ToList();
        }

        public async Task<int> Insert<T>(string query, T data)
        {
            var result = await _dbConnection.QuerySingleAsync<int>(query, data);
            return result;
        }

        public async Task<int> Update<T>(string query, T data)
        {
            var result = await _dbConnection.ExecuteAsync(query, data);
            return result;
        }

        public async Task<int> Delete(string query)
        {
            var result = await _dbConnection.ExecuteAsync(query);
            return result;
        }
        public async Task<int> Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            using IDbConnection db = new SqlConnection(_dbConnection.ConnectionString);
            return await db.ExecuteAsync(sp, parms, commandType: commandType);
        }

    }
}
