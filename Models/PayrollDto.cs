using EmployeeManagement.Modals;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeManagement.Models
{
    public class PayrollDto:ValidationErrors
    {
        public List<SelectListItem> DateRanges { get; set; } = new();
        public List<string> Dates { get; set; } = new();
        public List<Payroll> Payrolls { get; set; } = new();
        public double HourlyRate { get;set; }
        public string SelectedDate { get; set; }
        public bool IsDataSubmitted { get; set; }
    }
}
