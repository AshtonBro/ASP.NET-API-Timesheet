using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Timesheet.Api.Models;

namespace Timesheet.Api.Services
{
    public class ReportService
    {
        public EmployeeReport GetEmployeeReport(string lastName)
        {
            return new EmployeeReport
            { 
                LastName = lastName
            };
        }
    }
}
