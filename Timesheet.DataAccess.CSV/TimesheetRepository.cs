using System;
using System.Collections.Generic;
using System.IO;
using Timesheet.Domain;
using Timesheet.Domain.Models;

namespace Timesheet.DataAccess.CSV
{
    public class TimesheetRepository : ITimesheetRepository
    {
        private const string FILE_PATH = "..\\Timesheet.DataAccess.CSV\\Data\\timesheet.csv";
        private const char DELIMETER = ';';
        public void Add(TimeLog timeLog)
        {
            var dataRow = $"{timeLog.Comment}{DELIMETER}" +
                $"{timeLog.Date}{DELIMETER}" +
                $"{timeLog.LastName}{DELIMETER}" +
                $"{timeLog.WorkHours}\n";

            File.AppendAllText(FILE_PATH, dataRow);
        }

        public TimeLog[] GetTimesLog(string lastName)
        {
            var data = File.ReadAllText(FILE_PATH);

            var timeLogs = new List<TimeLog>();

            foreach (var dataRow in data.Split('\n'))
            {
                var timeLog = new TimeLog();

                var dataMembers = dataRow.Split(DELIMETER);

                timeLog.Comment = dataMembers[0];
                timeLog.Date = DateTime.TryParse(dataMembers[1], out var date) ? date : new DateTime();
                timeLog.LastName = dataMembers[2];
                timeLog.WorkHours = int.TryParse(dataMembers[3], out var workingHours) ? workingHours : 0;
            }

            return timeLogs.ToArray();
        }
    }
}
