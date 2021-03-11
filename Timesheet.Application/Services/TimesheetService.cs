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

        public bool TrackTime(TimeLog timeLog)
        {
            bool isValid = timeLog.WorkHours > 0 && timeLog.WorkHours <= 24 && !string.IsNullOrWhiteSpace(timeLog.LastName);

            isValid = UserSession.Sessions.Contains(timeLog.LastName) && isValid;

            if (!isValid)
            {
                return false;
            }

            _timesheetRepository.Add(timeLog);

            return true;
        }
    }
}