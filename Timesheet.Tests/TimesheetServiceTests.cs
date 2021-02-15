using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Timesheet.Tests
{
    class TimesheetServiceTests
    {
        [Test]
        public void TrackTime_ShouldReturnTrue()
        {
            // arrange
            var perid = new DateTime();
            var workingTimeHours = 1;
            var lastName = "";

            var service = new TimesheetService();
            // act

            var result = service.TrackTime();

            // assert
            Assert.IsTrue(result);
        }

        [Test]
        public void TrackTime_ShouldReturnFalse()
        {
            // arrange
            var perid = new DateTime();
            var workingTimeHours = 1;
            var lastName = "";

            var service = new TimesheetService();
           // act

           var result = service.TrackTime();

            // assert

            Assert.IsFalse(result);
        }
    }
}
