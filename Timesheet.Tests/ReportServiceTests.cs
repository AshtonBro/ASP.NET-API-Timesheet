using Moq;
using NUnit.Framework;
using System;
using Timesheet.Application.Services;
using Timesheet.Domain;
using Timesheet.Domain.Models;

namespace Timesheet.Tests
{
    class ReportServiceTests
    {
        [Test]
        public void GetEmployeeReport_ShouldReturnReport()
        {
            // arrange
            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var expectedLastName = "Иванов";


            timesheetRepositoryMock
                .Setup(x => x.GetTimesLog(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new[]
                {
                    new TimeLog
                    {
                        LastName = expectedLastName,
                        Date = DateTime.Now,
                        WorkHours = 10,
                        Comment = Guid.NewGuid().ToString()
                    }
                })
                .Verifiable();

            var service = new ReportService(timesheetRepositoryMock.Object);

            var expectedTotal = 100m;

            // act
            var result = service.GetEmployeeReport(expectedLastName);

            // assert
            timesheetRepositoryMock.VerifyAll();

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedLastName, result.LastName);

            Assert.IsNotNull(result.TimeLogs);
            Assert.IsNotEmpty(result.TimeLogs);

            Assert.AreEqual(expectedTotal, result.Bill);
        }
    }

}


