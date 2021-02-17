using System.Collections.Generic;
using System.Linq;

namespace Timesheet.Api.Services
{
    public class AuthService : IAuthService
    {
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

            var isEmployeeExist = Employee.Contains(lastName);
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
