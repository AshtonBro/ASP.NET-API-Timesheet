using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Timesheet.Api.Services
{
    public class AuthService
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

        public List<string> Employee { get; private set; }

        public bool Login(string lastName)
        {
            if(string.IsNullOrWhiteSpace(lastName))
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
}
