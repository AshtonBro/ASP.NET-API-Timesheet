﻿using System;

namespace Timesheet.Api.Models
{
    public class TimeLog
    {
        public DateTime Date { get; set; }
        public int WorkHours { get; set; }
        public string LastName { get; set; }
        public string Comment { get; set; }
    }
}