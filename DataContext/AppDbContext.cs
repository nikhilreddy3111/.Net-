using Microsoft.EntityFrameworkCore;
using EmployeeManagement.Modals;

namespace EmployeeManagement.DataContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<EmployeeManagement.Modals.Employee> Employee { get; set; } = default!;
        public DbSet<EmployeeManagement.Modals.Department> Department { get; set; } = default!;
    }
}
