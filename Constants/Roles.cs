using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Constants
{
    public enum Roles
    {
        [Display(Name = "Admin")]
        Admin,

        [Display(Name = "Employee")]
        Employee
    }
}
