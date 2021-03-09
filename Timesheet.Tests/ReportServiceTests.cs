﻿using Moq;
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
            var expectedTotalHourse = 20; // (8+8+4) / 160 * 70000

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
            Assert.AreEqual(expectedTotalHourse, result.TotalHours);
        }

        [Test]
        public void GetEmployeeReport_ShouldReturnReportPerSeveralMonth()
        {
            // arrange
            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var expectedLastName = "Иванов";
            var expectedTotal = 105750m; // 35 * 8 * 375 + 1 * 375 * 2;
            var expectedTotalHourse = 281; // (8+8+4) / 160 * 70000

            employeeRepositoryMock
                .Setup(x => x.GetEmployee(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new StaffEmployee
                {
                    LastName = expectedLastName,
                    Salary = 60000
                }
                )
                .Verifiable();

            timesheetRepositoryMock
               .Setup(x => x.GetTimesLog(It.Is<string>(y => y == expectedLastName)))
               .Returns(() =>
               {
                   TimeLog[] timeLogs = new TimeLog[35];
                   DateTime dateTime = new DateTime(2020, 11, 1);
                   timeLogs[0] = new TimeLog
                   {
                       LastName = expectedLastName,
                       Comment = Guid.NewGuid().ToString(),
                       Date = dateTime,
                       WorkHours = 9
                   };
                   for (int i = 1; i < timeLogs.Length; i++)
                   {
                       dateTime = dateTime.AddDays(1);
                       timeLogs[i] = new TimeLog
                       {
                           LastName = expectedLastName,
                           Comment = Guid.NewGuid().ToString(),
                           Date = dateTime,
                           WorkHours = 8
                       };
                   }
                   return timeLogs;
               })
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
            Assert.AreEqual(expectedTotalHourse, result.TotalHours);
        }

        [Test]
        public void GetEmployeeReport_WithoutTimeLogs_ShouldReturnReportPerSeveralMonth()
        {
            // arrange
            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var expectedLastName = "Иванов";
            var expectedTotal = 0m; // (8+8+4) / 160 * 70000
            var expectedTotalHourse = 0;

            timesheetRepositoryMock
                .Setup(x => x.GetTimesLog(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new TimeLog[0])
                .Verifiable();

            employeeRepositoryMock
                .Setup(x => x.GetEmployee(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new StaffEmployee
                {
                    LastName = expectedLastName,
                    Salary = 60000
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
            Assert.IsEmpty(result.TimeLogs);

            Assert.AreEqual(expectedTotal, result.Bill);
            Assert.AreEqual(expectedTotalHourse, result.TotalHours);
        }

        [Test]
        public void GetEmployeeReport_TimeLogsForOneDay_ShouldReturnReportPerSeveralMonth()
        {
            // arrange
            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var expectedLastName = "Иванов";
            var expectedTotal = 3000m; // (8+8+4) / 160 * 70000

            timesheetRepositoryMock
                .Setup(x => x.GetTimesLog(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new[]
                {
                    new TimeLog
                    {
                         LastName = expectedLastName,
                        Comment = Guid.NewGuid().ToString(),
                        Date = DateTime.Now.AddDays(-1),
                        WorkHours = 8
                    }
                })
                .Verifiable();

            employeeRepositoryMock
                .Setup(x => x.GetEmployee(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new StaffEmployee
                {
                    LastName = expectedLastName,
                    Salary = 60000
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

        [Test]
        public void GetEmployeeReport_TimeLogWithOvertimeForOneDay_ShouldReturnReportPerSeveralMonth()
        {
            // arrange
            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            var expectedLastName = "Иванов";
            var expectedTotal = 8m / 160m * 60000m + 4m / 160m * 60000m * 2; // (8+8+4) / 160 * 70000

            timesheetRepositoryMock
                .Setup(x => x.GetTimesLog(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new[]
                {
                    new TimeLog
                    {
                         LastName = expectedLastName,
                        Comment = Guid.NewGuid().ToString(),
                        Date = DateTime.Now.AddDays(-1),
                        WorkHours = 12
                    }
                })
                .Verifiable();

            employeeRepositoryMock
                .Setup(x => x.GetEmployee(It.Is<string>(y => y == expectedLastName)))
                .Returns(() => new StaffEmployee
                {
                    LastName = expectedLastName,
                    Salary = 60000
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


