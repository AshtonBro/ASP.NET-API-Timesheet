using System.Collections.Generic;
using System.Linq;
using Timesheet.Domain;
using Timesheet.Domain.Models;

namespace Timesheet.Application.Services
{
    public class ReportService : IReportService
    {
        private const decimal MAX_WORKING_HOURS_PER_MONTH = 160m;
        private const decimal MAX_WORKING_HOURS_PER_DAY = 8;
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IEmployeeRepository _employeeRepository;


        public ReportService(ITimesheetRepository timesheetRepository, IEmployeeRepository employeeRepository)
        {
            _timesheetRepository = timesheetRepository;
            _employeeRepository = employeeRepository;
        }

        public EmployeeReport GetEmployeeReport(string lastName)
        {
            var employee = _employeeRepository.GetEmployee(lastName);
            var timeLogs = _timesheetRepository.GetTimesLog(employee.LastName);

            if (timeLogs == null || timeLogs.Length == 0)
            {
                return new EmployeeReport
                {
                    LastName = employee.LastName
                };
            }

            var totalHourse = timeLogs.Sum(x => x.WorkHours);
            var workingHoursGroupsByDay = timeLogs.GroupBy(x => x.Date.ToShortDateString());
            decimal bill = 0;

            foreach (var workintLogsPerDay in workingHoursGroupsByDay)
            {
                var dayHours = workintLogsPerDay.Sum(x => x.WorkHours);

                if(dayHours > MAX_WORKING_HOURS_PER_DAY)
                {
                    var overtime = dayHours - MAX_WORKING_HOURS_PER_DAY;

                    bill += MAX_WORKING_HOURS_PER_DAY / MAX_WORKING_HOURS_PER_MONTH * employee.Salary;
                    bill += overtime / MAX_WORKING_HOURS_PER_MONTH * employee.Salary * 2;
                }
                else
                {
                    bill += dayHours / MAX_WORKING_HOURS_PER_MONTH * employee.Salary;
                }
            }

            return new EmployeeReport
            {
                LastName = employee.LastName,
                TimeLogs = timeLogs.ToList(),
                Bill = bill,
                TotalHours = totalHourse
            };
        }
    }
}
