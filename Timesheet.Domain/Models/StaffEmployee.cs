using System;
using System.Collections.Generic;

namespace Timesheet.Domain.Models
{
    /// <summary>
    /// Сотрудник
    /// </summary>
    public class StaffEmployee
    {
        public string LastName { get; set; }
        public decimal Salary { get; set; }
    }
}
