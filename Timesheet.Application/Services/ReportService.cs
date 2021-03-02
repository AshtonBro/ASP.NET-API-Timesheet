using System.Collections.Generic;
using System.Linq;
using Timesheet.Domain;
using Timesheet.Domain.Models;

namespace Timesheet.Application.Services
{
    public class ReportService
    {
        private readonly ITimesheetRepository _timesheetRepository;

        public ReportService(ITimesheetRepository timesheetRepository)
        {
            _timesheetRepository = timesheetRepository;
        }

        public EmployeeReport GetEmployeeReport(string lastName)
        {
            var timeLogs =_timesheetRepository.GetTimesLog(lastName);
            return new EmployeeReport
            {
                LastName = lastName,
                TimeLogs = timeLogs.ToList(),
                Bill = 0
            };
        }
    }
}
