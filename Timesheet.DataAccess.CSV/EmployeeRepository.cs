using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Timesheet.Domain;
using Timesheet.Domain.Models;

namespace Timesheet.DataAccess.CSV
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private const string _path = "\\timesheet.csv";
        private const char _delimeter = ';';

        public void AddEmployee(StaffEmployee staffEmployee)
        {
            var dataRow = $"{staffEmployee.LastName}{_delimeter}{staffEmployee.Salary}\n";

            File.AppendAllText(_path, dataRow);
        }

        public StaffEmployee GetEmployee(string lastName)
        {
            var data = File.ReadAllText(_path);

            StaffEmployee staffEmployee = null;

            foreach (var dataRow in data.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (dataRow.Contains(lastName))
                {
                    var dataMembers = dataRow.Split(_delimeter);

                    staffEmployee = new StaffEmployee(dataMembers[0], decimal.TryParse(dataMembers[1], out decimal salary) ? salary : 0);

                    break;
                }
            }
            return staffEmployee;
        }
    }
}
