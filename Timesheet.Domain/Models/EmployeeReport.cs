using System;
using System.Collections.Generic;

namespace Timesheet.Domain.Models
{
    /// <summary>
    /// Отчёт по сотруднику
    /// </summary>
    public class EmployeeReport
    {
        public string LastName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<TimeLog> TimeLogs { get; set; }


        public int TotalHours { get; set; }
        public decimal Bill { get; set; }
    }
}
