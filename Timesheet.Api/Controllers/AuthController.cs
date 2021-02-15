using Microsoft.AspNetCore.Mvc;
using Timesheet.Api.Models;
using Timesheet.Api.Services;

namespace Timesheet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public ActionResult<bool> Login(LoginRequest request)
        {
            var authService = new AuthService();

            return Ok(authService.Login(request.LastName));
        }
    }
}
