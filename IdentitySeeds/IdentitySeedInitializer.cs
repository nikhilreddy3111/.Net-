using EmployeeManagement.Constants;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace EmployeeManagement.IdentitySeeds
{
    public static class IdentitySeedInitializer
    {
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            foreach (string role in Enum.GetNames(typeof(Roles)))
            {
                if (!roleManager.RoleExistsAsync(role).Result)
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }
    }
}
