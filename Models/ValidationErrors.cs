namespace EmployeeManagement.Models
{
    public class ValidationErrors
    {
        public List<string> Errors { get; set; } = new();
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
    }
}
