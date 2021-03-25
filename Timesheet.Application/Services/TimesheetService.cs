using System;
using Timesheet.Domain;
using Timesheet.Domain.Models;
using static Timesheet.Application.Services.AuthService;

namespace Timesheet.Application.Services
{
    public class TimesheetService : ITimesheetService
    {
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public TimesheetService(ITimesheetRepository timesheetRepository, IEmployeeRepository employeeRepository)
        {
            _timesheetRepository = timesheetRepository;
            _employeeRepository = employeeRepository;
        }

        public bool TrackTime(TimeLog timeLog, string lastName)
        {
            bool isValid = timeLog.WorkHours > 0
                && timeLog.WorkHours <= 24 
                && !string.IsNullOrWhiteSpace(timeLog.LastName);


            var employee = _employeeRepository.GetEmployee(timeLog.LastName);
            isValid = UserSessions.Sessions.Contains(timeLog.LastName) && isValid;

            if (!isValid)
            {
                return false;
            }

            if (DateTime.Now.AddDays(-2) > timeLog.Date)
            {
                return false;
            }

            _timesheetRepository.Add(timeLog);

            return true;
        }
    }
}