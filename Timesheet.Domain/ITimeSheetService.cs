using Timesheet.Domain.Models;

namespace Timesheet.Domain
{
    public interface ITimesheetService
    {
        bool TrackTime(TimeLog timelog, string lastName);
    }
}
