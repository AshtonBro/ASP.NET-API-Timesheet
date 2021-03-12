﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Timesheet.Domain;
using Timesheet.Domain.Models;

namespace Timesheet.DataAccess.CSV
{
    public class TimesheetRepository : ITimesheetRepository
    {
        private const string _path = "\\timesheet.csv";
        private const char _delimeter = ';';
        public void Add(TimeLog timeLog)
        {
            var dataRow = $"{timeLog.Comment}{_delimeter}" +
                 $"{timeLog.Date}{_delimeter}" +
                 $"{timeLog.LastName}{_delimeter}" +
                 $"{timeLog.WorkHours}\n";

            File.AppendAllText(_path, dataRow);
        }

        public TimeLog[] GetTimesLog(string lastName)
        {
            var data = File.ReadAllText(_path);

            var timeLogs = new List<TimeLog>();

            foreach (var dataRow in data.Split('\n'))
            {
                var timeLog = new TimeLog();

                var dataMembers = dataRow.Split(_delimeter);

                timeLog.Comment = dataMembers[0];
                timeLog.Date = DateTime.TryParse(dataMembers[1], out var date) ? date : new DateTime();
                timeLog.LastName = dataMembers[2];
                timeLog.WorkHours = int.TryParse(dataMembers[3], out var workingHours) ? workingHours : 0;
            }

            return timeLogs.ToArray();
        }
    }
}