using Dapper;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace EmployeeManagement.DapperContent
{
    public class DapperRoleStore : IRoleStore<IdentityRole>
    {
        private readonly IDbConnection _dbConnection;
        public DapperRoleStore(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task<IdentityResult> CreateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            const string sql = "INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp) VALUES (@Id, @Name, @NormalizedName, @ConcurrencyStamp)";
            var result = await _dbConnection.ExecuteAsync(sql, role);
            return result > 0 ? IdentityResult.Success : IdentityResult.Failed();
        }

        public async Task<IdentityResult> DeleteAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            const string sql = "DELETE FROM AspNetRoles WHERE Id = @Id";
            var result = await _dbConnection.ExecuteAsync(sql, new { role.Id });
            return result > 0 ? IdentityResult.Success : IdentityResult.Failed();
        }
        public async Task<IdentityRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            const string sql = "SELECT * FROM AspNetRoles WHERE Id = @Id";
            return await _dbConnection.QuerySingleOrDefaultAsync<IdentityRole>(sql, new { Id = roleId });
        }
        public async Task<IdentityRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            const string sql = "SELECT * FROM AspNetRoles WHERE NormalizedName = @NormalizedName";
            return await _dbConnection.QuerySingleOrDefaultAsync<IdentityRole>(sql, new { NormalizedName = normalizedRoleName });
        }
        public async Task<IdentityResult> UpdateAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            const string sql = @"UPDATE AspNetRoles SET Name = @Name, NormalizedName = @NormalizedName, ConcurrencyStamp = @ConcurrencyStamp WHERE Id = @Id";
            var result = await _dbConnection.ExecuteAsync(sql, role); return result > 0 ? IdentityResult.Success : IdentityResult.Failed();
        }
        public Task<string> GetRoleIdAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Id);
        }
        public Task<string> GetRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.Name);
        }
        public Task SetRoleNameAsync(IdentityRole role, string roleName, CancellationToken cancellationToken)
        {
            role.Name = roleName;
            return Task.CompletedTask;
        }
        public Task<string> GetNormalizedRoleNameAsync(IdentityRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedName);
        }
        public Task SetNormalizedRoleNameAsync(IdentityRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            
        }
    }
}
