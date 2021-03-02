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
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var expectedLastName = "Иванов";
            var expectedTotal = 8750m; // (8+8+4) / 160 * 70000
            
            timesheetRepositoryMock
                .Setup(x => x.GetTimesLog(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new[]
                {
                    new TimeLog
                    {
                        LastName = expectedLastName,
                        Date = DateTime.Now.AddDays(-2),
                        WorkHours = 8,
                        Comment = Guid.NewGuid().ToString()
                    },
                     new TimeLog
                    {
                        LastName = expectedLastName,
                        Date = DateTime.Now.AddDays(-1),
                        WorkHours = 8,
                        Comment = Guid.NewGuid().ToString()
                    },
                      new TimeLog
                    {
                        LastName = expectedLastName,
                        Date = DateTime.Now,
                        WorkHours = 4,
                        Comment = Guid.NewGuid().ToString()
                    }
                })
                .Verifiable();

            employeeRepositoryMock
                .Setup(x => x.GetEmployee(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new StaffEmployee
                    {
                        LastName = expectedLastName,
                        Salary = 70000
                    }
                )
                .Verifiable();

            var service = new ReportService(timesheetRepositoryMock.Object, employeeRepositoryMock.Object);


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


