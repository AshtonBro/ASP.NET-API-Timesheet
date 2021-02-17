using System.Collections.Generic;

namespace Timesheet.Api.Services
{
    public interface IAuthService
    {
        IEnumerable<string> Employee { get; }

        bool Login(string lastName);
    }
}