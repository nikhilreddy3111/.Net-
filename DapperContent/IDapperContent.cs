using Dapper;
using System.Data;

namespace EmployeeManagement.DapperContent
{
    public interface IDapperContent
    {
        T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        List<T> GetAll<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
        Task<int> Insert<T>(string query, T data);
        Task<int> Update<T>(string query, T data);
        Task<int> Delete(string query);
        Task<int> Execute(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure);
    }
}
