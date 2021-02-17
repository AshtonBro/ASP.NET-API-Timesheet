using System;
using System.Collections.Generic;
using Timesheet.Api.Models;

namespace Timesheet.Api.Services
{
    public class TimesheetService
    {
        public bool TrackTime(TimeLog timelog)
        {
            bool isValid = timelog.WorkHours > 0 && timelog.WorkHours <= 24
                && !string.IsNullOrWhiteSpace(timelog.LastName);

            isValid = isValid && UserSession.Sessions.Contains(timelog.LastName);

            if (!isValid)
            {
                return false;
            }

            Timesheet.TimeLogs.Add(timelog);

            return true;
        }
    }

    public static class Timesheet
    {
        public static List<TimeLog> TimeLogs { get; set; } = new List<TimeLog>();
    }
}
