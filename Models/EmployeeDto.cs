using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagement.Models
{
    public class EmployeeDto: ValidationErrors
    {
        public int Id { get; set; }
        public string AspNetUserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        [MinLength(8)]
        public string Password { get; set; }
        [DisplayName("Confirm Password")]
        [MinLength(8)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public string Role { get; set; }
        public int DepartmentId { get; set; }
        [DisplayName("Department")]
        public string DepartmentName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastLoggedIn { get; set; }
        public List<SelectListItem> Departments { get; set; }
        public int VacationId { get; set; }
        [DisplayName("Number of Vacation Days")]
        public int NumberOfDays { get; set; }
    }
}
