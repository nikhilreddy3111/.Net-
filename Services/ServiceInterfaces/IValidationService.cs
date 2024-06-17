using EmployeeManagement.Models;

namespace EmployeeManagement.Services.ServiceInterfaces
{
    public interface IValidationService
    {
        List<string> ValidateEmployee(EmployeeDto employee);
    }
}
