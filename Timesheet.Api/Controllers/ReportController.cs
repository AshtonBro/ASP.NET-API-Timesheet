using Microsoft.AspNetCore.Mvc;
using Timesheet.Domain;
using Timesheet.Domain.Models;

namespace Timesheet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        public readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public ActionResult<EmployeeReport> Report(string lastName)
        {
            return Ok(_reportService.GetEmployeeReport(lastName));
        }
    }
}
