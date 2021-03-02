using Timesheet.Domain.Models;

namespace Timesheet.Domain
{
    public interface ITimesheetRepository
    {
        TimeLog[] GetTimesLog(string lastName);
    }
}
