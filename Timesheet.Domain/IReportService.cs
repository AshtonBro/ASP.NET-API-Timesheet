namespace Timesheet.Domain.Models
{
    public interface IReportService
    {
        EmployeeReport GetEmployeeReport(string lastName);
    }
}
