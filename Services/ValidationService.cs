using EmployeeManagement.Models;
using EmployeeManagement.Services.ServiceInterfaces;
using System.Text.RegularExpressions;

namespace EmployeeManagement.Services
{
    public class ValidationService: IValidationService
    {
        public List<string> ValidateEmployee(EmployeeDto employee)
        {
            List<string> errors = new List<string>();
            if (employee != null)
            {
                if (string.IsNullOrWhiteSpace(employee.Name))
                {
                    errors.Add("Please enter Name of the Employee");
                }
                if (string.IsNullOrWhiteSpace(employee.Email))
                {
                    errors.Add("Please enter Email of the Employee");
                }
                if (employee.DepartmentId <= 0)
                {
                    errors.Add("Please select Department of the Employee");
                }
                if (!string.IsNullOrWhiteSpace(employee.PhoneNumber) && !Regex.IsMatch(employee.PhoneNumber, "[0-9]{3}-[0-9]{3}-[0-9]{4}"))
                {
                    errors.Add("Please enter Phone Number of the Employee(xxx-xxx-xxxx) in the given format");
                }
                if (string.IsNullOrWhiteSpace(employee.Password) || string.IsNullOrWhiteSpace(employee.ConfirmPassword))
                {
                    errors.Add("Password cannot be empty and it should be minimum length of 8");
                }
            }
            return errors;
        }
    }
}
