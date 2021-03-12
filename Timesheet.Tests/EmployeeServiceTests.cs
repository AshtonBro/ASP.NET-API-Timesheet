using Moq;
using NUnit.Framework;
using Timesheet.Application.Services;
using Timesheet.Domain;
using Timesheet.Domain.Models;

namespace Timesheet.Tests
{
    class EmployeeServiceTests
    {
        [Test]
        [TestCase("Иванов", 20000)]
        [TestCase("Петров", 30000)]
        [TestCase("Сидоров", 40000)]
        public void Add_ShouldReturnTrue(string lastName, int salary)
        {
            // Arrange
            var staffEmployee = new StaffEmployee(lastName, salary);

            var employeeRepository = new Mock<IEmployeeRepository>();
            employeeRepository.Setup(x => x.AddEmployee(staffEmployee))
                .Verifiable();

            var service = new EmployeeService(employeeRepository.Object);

            //Act
            var result = service.AddEmployee(staffEmployee);

            //Assert
            employeeRepository.Verify(x => x.AddEmployee(staffEmployee), Times.Once);
            Assert.IsTrue(result);
        }

        [Test]
        [TestCase("Иванов", 0)]
        [TestCase("Петров", -1000)]
        [TestCase("", 40000)]
        [TestCase(null, 40000)]
        public void Add_ShouldReturnFalse(string lastName, int salary)
        {
            // Arrange
            var staffEmployee = new StaffEmployee(lastName, salary);

            var employeeRepository = new Mock<IEmployeeRepository>();
            employeeRepository.Setup(x => x.AddEmployee(staffEmployee))
                .Verifiable();

            var service = new EmployeeService(employeeRepository.Object);

            //Act
            var result = service.AddEmployee(staffEmployee);

            //Assert
            employeeRepository.Verify(x => x.AddEmployee(It.IsAny<StaffEmployee>()), Times.Never);
            Assert.IsFalse(result);
        }
    }
}
