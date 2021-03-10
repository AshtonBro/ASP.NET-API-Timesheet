using Timesheet.Domain;
using Timesheet.Domain.Models;

namespace Timesheet.Application.Services
{
    public class TimesheetService : ITimesheetService
    {
        private readonly ITimesheetRepository _timesheetRepository;
        public TimesheetService(ITimesheetRepository timesheetRepository)
        {
            _timesheetRepository = timesheetRepository;
        }
        public bool TrackTime(TimeLog timelog)
        {
            bool isValid = timelog.WorkHours > 0 && timelog.WorkHours <= 24
            && !string.IsNullOrWhiteSpace(timelog.LastName);

            isValid = isValid && UserSession.Sessions.Contains(timelog.LastName);

            if (!isValid)
            {
                return false;
            }

            _timesheetRepository.Add(timelog);

            return true;
        }
    }
}
