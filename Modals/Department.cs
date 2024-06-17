using EmployeeManagement.Models;

namespace EmployeeManagement.Modals
{
    public class Department: ValidationErrors
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
