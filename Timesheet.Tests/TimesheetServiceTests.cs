using Moq;
using NUnit.Framework;
using System;
using Timesheet.Application.Services;
using Timesheet.Domain;
using Timesheet.Domain.Models;
using static Timesheet.Application.Services.AuthService;

namespace Timesheet.Tests
{
    class TimesheetServiceTests
    {
        [SetUp]
        public void SetUp()
        {
            UserSessions.Sessions.Clear();
        }

        [Test]
        public void TrackTime_StaffEmployee_ShouldReturnTrue()
        {
            // arrange

            var expectedLastName = "TestUser";

            UserSessions.Sessions.Add(expectedLastName);

            var timeLog = new TimeLog
            {
                Date = DateTime.Now.AddDays(-10),
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
        public void TrackTime_StaffEmployee_ShouldReturnFalse(int hours, string lastName)
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

        [Test]
        public void TrackTime_Freelancer_ShouldReturnTrue()
        {
            // arrange
            var expectedLastName = "TestUser";

            UserSessions.Sessions.Add(expectedLastName);

            var timeLog = new TimeLog
            {
                Date = DateTime.Now,
                WorkHours = 2,
                LastName = expectedLastName,
                Comment = Guid.NewGuid().ToString()
            };

            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();

            var service = new TimesheetService(timesheetRepositoryMock.Object);
            // act

            var result = service.TrackTime(timeLog);

            // assert
            var lowerBorderDate = DateTime.Now.AddDays(-2);

            //timesheetRepositoryMock.Verify(x => x.Add(It.Is<TimeLog>(y => y.LastName == "")));
            timesheetRepositoryMock.Verify(x => x.Add(timeLog), Times.Once);

            Assert.True(timeLog.Date > lowerBorderDate);
            Assert.True(result);
        }

        [Test]
        public void TrackTime_Freelancer_ShouldReturnFalse()
        {
            // arrange
            var expectedLastName = "TestUser";

            UserSessions.Sessions.Add(expectedLastName);

            var timeLog = new TimeLog
            {
                Date = DateTime.Now.AddDays(-3),
                WorkHours = 2,
                LastName = expectedLastName,
                Comment = Guid.NewGuid().ToString()
            };

            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();

            var service = new TimesheetService(timesheetRepositoryMock.Object);
            // act

            var result = service.TrackTime(timeLog);

            // assert
            var lowerBorderDate = DateTime.Now.AddDays(-2);

            Assert.False(timeLog.Date > lowerBorderDate);
            Assert.False(result);
        }
    }
}
