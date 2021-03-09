using Moq;
using NUnit.Framework;
using System;
using Timesheet.Application.Services;
using Timesheet.Domain;
using Timesheet.Domain.Models;

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

            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();

            timesheetRepositoryMock
                .Setup(x => x.Add(timeLog))
                .Verifiable();

            var service = new TimesheetService(timesheetRepositoryMock.Object);
            // act

            var result = service.TrackTime(timeLog);

            // assert
            timesheetRepositoryMock.Verify(x => x.Add(timeLog), Times.Once);
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

            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();

            timesheetRepositoryMock
                .Setup(x => x.Add(timeLog))
                .Verifiable();

            var service = new TimesheetService(timesheetRepositoryMock.Object);
            // act

            var result = service.TrackTime(timeLog);

            // assert
            timesheetRepositoryMock.Verify(x => x.Add(timeLog), Times.Never);
            Assert.IsFalse(result);
        }
    }
}
