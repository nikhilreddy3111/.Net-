using Dapper;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace EmployeeManagement.DapperContent
{
    public class DapperUserStore : IUserStore<IdentityUser>, IUserPasswordStore<IdentityUser>
    {
        private readonly IDbConnection _dbConnection;
        public DapperUserStore(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }
        public async Task<IdentityResult> CreateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            const string sql = @"INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount) 
                    VALUES (@Id, @UserName, @NormalizedUserName, @Email, @NormalizedEmail, @EmailConfirmed, @PasswordHash, @SecurityStamp, @ConcurrencyStamp, @PhoneNumber, @PhoneNumberConfirmed, @TwoFactorEnabled, @LockoutEnd, @LockoutEnabled, @AccessFailedCount)";
            var result = await _dbConnection.ExecuteAsync(sql, user);
            return result > 0 ? IdentityResult.Success : IdentityResult.Failed();
        }
        public async Task<IdentityResult> DeleteAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            const string sql = "DELETE FROM AspNetUsers WHERE Id = @Id";
            var result = await _dbConnection.ExecuteAsync(sql, new { user.Id });
            return result > 0 ? IdentityResult.Success : IdentityResult.Failed();
        }
        public async Task<IdentityUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            const string sql = "SELECT * FROM AspNetUsers WHERE Id = @Id";
            return await _dbConnection.QuerySingleOrDefaultAsync<IdentityUser>(sql, new { Id = userId });
        }
        public async Task<IdentityUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            const string sql = "SELECT * FROM AspNetUsers WHERE NormalizedUserName = @NormalizedUserName";
            return await _dbConnection.QuerySingleOrDefaultAsync<IdentityUser>(sql, new { NormalizedUserName = normalizedUserName });
        }
        public async Task<IdentityResult> UpdateAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            const string sql = @"UPDATE AspNetUsers SET UserName = @UserName, NormalizedUserName = @NormalizedUserName, Email = @Email, NormalizedEmail = @NormalizedEmail, EmailConfirmed = @EmailConfirmed, PasswordHash = @PasswordHash, SecurityStamp = @SecurityStamp, ConcurrencyStamp = @ConcurrencyStamp, PhoneNumber = @PhoneNumber, PhoneNumberConfirmed = @PhoneNumberConfirmed, TwoFactorEnabled = @TwoFactorEnabled, LockoutEnd = @LockoutEnd, LockoutEnabled = @LockoutEnabled, AccessFailedCount = @AccessFailedCount WHERE Id = @Id";
            var result = await _dbConnection.ExecuteAsync(sql, user);
            return result > 0 ? IdentityResult.Success : IdentityResult.Failed();
        }
        public Task<string> GetUserIdAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }
        public Task<string> GetUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }
        public Task SetUserNameAsync(IdentityUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }
        public Task<string> GetNormalizedUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }
        public Task SetNormalizedUserNameAsync(IdentityUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }
        public Task SetPasswordHashAsync(IdentityUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }
        public Task<string> GetPasswordHashAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }
        public Task<bool> HasPasswordAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }
        public void Dispose()
        {
            
        }
    }
}