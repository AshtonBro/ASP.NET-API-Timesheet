﻿using Microsoft.AspNetCore.Mvc;
using Timesheet.Api.ResourceModels;
using Timesheet.Api.Services;

namespace Timesheet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }


        [HttpPost]
        public ActionResult<bool> Login(LoginRequest request)
        {
            return Ok(_authService.Login(request.LastName));
        }
    }
}
