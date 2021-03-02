using System.Collections.Generic;
using System.Linq;
using Timesheet.Domain;
using Timesheet.Domain.Models;

namespace Timesheet.Application.Services
{
    public class ReportService
    {
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
            var timeLogs =_timesheetRepository.GetTimesLog(employee.LastName);

            //foreach (var log in timeLogs)
            //{
            //    hours += log.WorkHours;
            //}

            var hours = timeLogs.Sum(x => x.WorkHours);
            var bill = (hours / 160m) * employee.Salary;

            return new EmployeeReport
            {
                LastName = employee.LastName,
                TimeLogs = timeLogs.ToList(),
                Bill = bill
            };
        }
    }
}
