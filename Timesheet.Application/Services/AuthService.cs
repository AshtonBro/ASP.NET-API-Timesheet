using System.Collections.Generic;
using Timesheet.Domain;

namespace Timesheet.Application.Services
{
    public class AuthService : IAuthService
    {
       private readonly IEmployeeRepository _employeeRepository;
        public AuthService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public AuthService()
        {
            Employee = new List<string>
            {
                "Иванов",
                "Петров",
                "Сидоров"
            };
        }

        public IEnumerable<string> Employee { get; private set; }

        public bool Login(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
            {
                return false;
            }

            var stafffEmployee = _employeeRepository.GetEmployee(lastName);
            var isEmployeeExist = stafffEmployee != null;

            if (isEmployeeExist)
            {
                UserSession.Sessions.Add(lastName);
            }

            return isEmployeeExist;
        }
    }
    public static class UserSession
    {
        public static HashSet<string> Sessions { get; set; } = new HashSet<string>();
    }
}
