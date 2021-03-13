using Moq;
using NUnit.Framework;
using Timesheet.Application.Services;
using Timesheet.Domain;
using Timesheet.Domain.Models;

namespace Timesheet.Tests
{
    public class AuthServiceTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("������")]
        [TestCase("������")]
        [TestCase("�������")]
        public void Login_ShouldReturnTrue(string lastName)
        {
            //arrange
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.
                Setup(x => x.GetEmployee(It.Is<string>(y => y == lastName)))
                .Returns(() => new StaffEmployee(lastName, 70000))
                .Verifiable();

            var service = new AuthService(employeeRepositoryMock.Object);
            //act

            var result = service.Login(lastName);

            //assert
            employeeRepositoryMock.VerifyAll();

            Assert.IsNotNull(UserSession.Sessions);
            Assert.IsNotEmpty(UserSession.Sessions);
            Assert.IsTrue(UserSession.Sessions.Contains(lastName));
            Assert.IsTrue(result);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("TestUser")]
        public void Login_ShouldReturnFalse(string lastName)
        {
            //arrange
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.
                Setup(x => x.GetEmployee(lastName))
                .Returns(() => null);

            var service = new AuthService(employeeRepositoryMock.Object);
            //act

            var result = service.Login(lastName);

            //assert
            if(string.IsNullOrWhiteSpace(lastName) == false)
            employeeRepositoryMock.Verify(x => x.GetEmployee(lastName), Times.Once);

            Assert.IsFalse(result);
            Assert.IsEmpty(UserSession.Sessions);
            Assert.IsTrue(UserSession.Sessions.Contains(lastName) == false);
        }


        public void Login_InvokeLoginTwiceForOneLastName_ShouldReturnTrue()
        {
            //arrange
            string lastName = "������";
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();
            employeeRepositoryMock.
                Setup(x => x.GetEmployee(It.Is<string>(y => y == lastName)))
                .Returns(() => new StaffEmployee(lastName, 70000))
                .Verifiable();

            var service = new AuthService(employeeRepositoryMock.Object);

            //act

            var result = service.Login(lastName);
            result = service.Login(lastName);

            //assert
            employeeRepositoryMock.VerifyAll();

            Assert.IsNotNull(UserSession.Sessions);
            Assert.IsNotEmpty(UserSession.Sessions);
            Assert.IsTrue(UserSession.Sessions.Contains(lastName));
            Assert.IsTrue(result);
        }

        [TestCase(null)]
        [TestCase("")]
        public void Login_NotValidArgument_ShouldReturnFalse(string lastName)
        {
            //arrange
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            var service = new AuthService(employeeRepositoryMock.Object);

            //act
            var result = service.Login(lastName);

            //assert
            employeeRepositoryMock.Verify(x => x.GetEmployee(lastName), Times.Never);

            Assert.IsFalse(result);
            Assert.IsEmpty(UserSession.Sessions);
            Assert.IsTrue(UserSession.Sessions.Contains(lastName) == false);
        }

        [TestCase("TestUser")]
        public void Login_UserDoesntExist_ShouldReturnFalse(string lastName)
        {
            //arrange
            var employeeRepositoryMock = new Mock<IEmployeeRepository>();

            employeeRepositoryMock.
                Setup(x => x.GetEmployee(It.Is<string>(y => y == lastName)))
                .Returns(() => null);

            var service = new AuthService(employeeRepositoryMock.Object);

            //act
            var result = service.Login(lastName);

            //assert

            employeeRepositoryMock.Verify(x => x.GetEmployee(lastName), Times.Once);

            Assert.IsFalse(result);
            Assert.IsTrue(UserSession.Sessions.Contains(lastName) == false);
        }
    }
}