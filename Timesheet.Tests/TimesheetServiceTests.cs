using NUnit.Framework;
using System;
using Timesheet.Api.Models;
using Timesheet.Api.Services;

namespace Timesheet.Tests
{
    class TimesheetServiceTests
    {
        [Test]
        public void TrackTime_ShouldReturnTrue()
        {
            // arrange
            var expectedLastName = "TestUser";

            UserSession.Sessions.Add(expectedLastName);

            var timeLog = new TimeLog
            {
                Date = new DateTime(),
                WorkHours = 1,
                LastName = expectedLastName,
                Comment = Guid.NewGuid().ToString()
            };

            var service = new TimesheetService();
            // act

            var result = service.TrackTime(timeLog);

            // assert
            Assert.IsTrue(result);
        }

        [TestCase(25, "TestUser")]
        [TestCase(25, null)]
        [TestCase(25, "")]
        [TestCase(-1, "TestUser")]
        [TestCase(-1, null)]
        [TestCase(-1, "")]
        [TestCase(4, "TestUser")]
        [TestCase(4, null)]
        [TestCase(4, "")]
        public void TrackTime_ShouldReturnFalse(int hours, string lastName)
        {
            // arrange
            var timeLog = new TimeLog
            {
                Date = new DateTime(),
                WorkHours = hours,
                LastName = lastName
            };

            var service = new TimesheetService();
           // act

           var result = service.TrackTime(timeLog);

            // assert

            Assert.IsFalse(result);
        }
    }
}
