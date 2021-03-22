using Microsoft.AspNetCore.Mvc;
using Timesheet.Domain;
using Timesheet.Domain.Models;

namespace Timesheet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FreelancerEmployeeController : ControllerBase
    {
        public readonly IEmployeeService _employeeService;

        public FreelancerEmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        public ActionResult<bool> AddEmployee(FreelancerEmployee freelancerEmployee)
        {
            return Ok(_employeeService.AddEmployee(freelancerEmployee));
        }
    }
}
