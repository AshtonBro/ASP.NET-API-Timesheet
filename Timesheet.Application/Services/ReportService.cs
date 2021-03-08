using System.Collections.Generic;
using System.Linq;
using Timesheet.Domain;
using Timesheet.Domain.Models;

namespace Timesheet.Application.Services
{
    public class ReportService
    {
        private const decimal MAX_WORKING_HOURS_PER_MONTH = 160m;
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


            var monthHours = timeLogs[0].WorkHours;
            var billPerHour = employee.Salary / MAX_WORKING_HOURS_PER_MONTH;
            var bill = monthHours * billPerHour;
            var totalHourse = timeLogs.Sum(x => x.WorkHours);

            for (int i = 1; i < timeLogs.Length; i++)
            {
                var dayHours = timeLogs[i].WorkHours;

                if (timeLogs[i].Date.Month != timeLogs[i - 1].Date.Month)
                {
                    monthHours = 0;
                }

                monthHours += dayHours;

                if (monthHours <= MAX_WORKING_HOURS_PER_MONTH)
                {
                    bill += timeLogs[i].WorkHours * billPerHour;
                }
                else if (monthHours < (MAX_WORKING_HOURS_PER_MONTH + 8))
                {
                    var overWorkHours = monthHours - MAX_WORKING_HOURS_PER_MONTH;
                    var simpleWorkHours = dayHours - overWorkHours;
                    bill += simpleWorkHours * billPerHour;
                    bill += overWorkHours * billPerHour * 2;
                }
                else
                {
                    bill += timeLogs[i].WorkHours * billPerHour * 2;
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
