using Microsoft.AspNetCore.Mvc;
using Timesheet.Domain;
using Timesheet.Domain.Models;

namespace Timesheet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChiefEmployeeController : ControllerBase
    {
        public readonly IEmployeeService _employeeService;

        public ChiefEmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
         
        [HttpPost]
        public ActionResult<bool> AddEmployee(ChiefEmployee chiefEmployee)
        {
            return Ok(_employeeService.AddEmployee(chiefEmployee));
        }
    }
}
