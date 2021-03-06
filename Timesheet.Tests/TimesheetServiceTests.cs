﻿using Moq;
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
        private readonly TimesheetService _service;
        private readonly Mock<ITimesheetRepository> _timesheetRepositoryMock; 
        private readonly Mock<IEmployeeRepository> _employeeRepositoryMock; 

        public TimesheetServiceTests()
        {
            _timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            _employeeRepositoryMock = new Mock<IEmployeeRepository>();

            _service = new TimesheetService(_timesheetRepositoryMock.Object, _employeeRepositoryMock.Object);
        }

        [SetUp]
        public void SetUp()
        {
            UserSessions.Sessions.Clear();
        }

        [Test]
        public void TrackTime_StaffEmployeeTrackPreviousTime_ShouldReturnTrue()
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

            _employeeRepositoryMock
              .Setup(x => x.GetEmployee(expectedLastName))
              .Returns(() => new StaffEmployee(expectedLastName, 0m))
              .Verifiable();

            // act
            var result = _service.TrackTime(timeLog, expectedLastName);

            // assert
            _employeeRepositoryMock.VerifyAll();
            _timesheetRepositoryMock.Verify(x => x.Add(timeLog), Times.Once);
            Assert.IsTrue(result);
        }

        [Test]
        public void TrackTime_StaffEmployee_ShouldReturnTrue()
        {
            // arrange
            var expectedLastName = "TestUser";

            UserSessions.Sessions.Add(expectedLastName);

            var timeLog = new TimeLog
            {
                Date = DateTime.Now,
                WorkHours = 1,
                LastName = expectedLastName,
                Comment = Guid.NewGuid().ToString()
            };

            _timesheetRepositoryMock
                .Setup(x => x.Add(timeLog));

            _employeeRepositoryMock
             .Setup(x => x.GetEmployee(expectedLastName))
             .Returns(() => new StaffEmployee(expectedLastName, 0m))
             .Verifiable();

            // act
            var result = _service.TrackTime(timeLog, expectedLastName);

            // assert
            _employeeRepositoryMock.VerifyAll();
            _timesheetRepositoryMock.Verify(x => x.Add(timeLog), Times.Once);
            Assert.IsTrue(result);
        }

        public void TrackTime_ChiefEmployee_ShouldReturnTrue()
        {
            // arrange
            var expectedLastName = "TestUser";

            UserSessions.Sessions.Add(expectedLastName);

            var timeLog = new TimeLog
            {
                Date = DateTime.Now,
                WorkHours = 1,
                LastName = expectedLastName,
                Comment = Guid.NewGuid().ToString()
            };

            _timesheetRepositoryMock
                .Setup(x => x.Add(timeLog));

            _employeeRepositoryMock
             .Setup(x => x.GetEmployee(expectedLastName))
             .Returns(() => new ChiefEmployee(expectedLastName, 0m, 0m))
             .Verifiable();

            // act
            var result = _service.TrackTime(timeLog, expectedLastName);

            // assert
            _employeeRepositoryMock.VerifyAll();
            _timesheetRepositoryMock.Verify(x => x.Add(timeLog), Times.Once);
            Assert.IsTrue(result);
        }

        public void TrackTime_StaffEmployeeTriesAddWrongLastName_ShouldReturnFalse()
        {
            // arrange
            var expectedLastName = "TestUser";

            UserSessions.Sessions.Add(expectedLastName);

            var timeLog = new TimeLog
            {
                Date = DateTime.Now,
                WorkHours = 1,
                LastName = Guid.NewGuid().ToString(),
                Comment = Guid.NewGuid().ToString()
            };

            _employeeRepositoryMock
             .Setup(x => x.GetEmployee(expectedLastName))
             .Returns(() => new StaffEmployee(expectedLastName, 0m))
             .Verifiable();

            // act
            var result = _service.TrackTime(timeLog, expectedLastName);

            // assert
            _employeeRepositoryMock.VerifyAll();
            _timesheetRepositoryMock.Verify(x => x.Add(timeLog), Times.Never);
            Assert.IsFalse(result);
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

            _employeeRepositoryMock
               .Setup(x => x.GetEmployee(lastName))
               .Returns(() => null)
               .Verifiable();

            // act
            var result = _service.TrackTime(timeLog, timeLog.LastName);

            // assert
            _employeeRepositoryMock.VerifyAll();
            _timesheetRepositoryMock.Verify(x => x.Add(timeLog), Times.Never);
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

            _employeeRepositoryMock
                .Setup(x => x.GetEmployee(expectedLastName))
                .Returns(() => new FreelancerEmployee(expectedLastName, 0m))
                .Verifiable();

            // act
            var result = _service.TrackTime(timeLog, expectedLastName);

            // assert
            var lowerBorderDate = DateTime.Now.AddDays(-2);

            _employeeRepositoryMock.VerifyAll();
            _timesheetRepositoryMock.Verify(x => x.Add(timeLog), Times.Once);

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

            _employeeRepositoryMock
               .Setup(x => x.GetEmployee(expectedLastName))
               .Returns(() => new FreelancerEmployee(expectedLastName, 0m))
               .Verifiable();

            // act
            var result = _service.TrackTime(timeLog, expectedLastName);

            // assert
            Assert.IsFalse(result);

            _employeeRepositoryMock.VerifyAll();
            _timesheetRepositoryMock.Verify(x => x.Add(timeLog), Times.Never);
        }
    }
}
