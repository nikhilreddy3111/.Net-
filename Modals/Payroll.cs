namespace EmployeeManagement.Modals
{
    public class Payroll
    {
        public int Id { get; set; }
        public string AspNetUserId { get; set; }
        public double HoursWorked { get; set; }
        public double HourlyRate { get; set; }
        public DateTime WorkedDate { get; set; }
        public string WeekDay { get; set; }
    }
}
