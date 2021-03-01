using System.Collections.Generic;

namespace Timesheet.Domain
{
    public interface IAuthService
    {
        IEnumerable<string> Employee { get; }

        bool Login(string lastName);
    }
}