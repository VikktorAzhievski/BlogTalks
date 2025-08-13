using BlogTalks.Application.Users.Commands;
using BlogTalks.Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog.Core;

namespace BlogTalks.API.Controllers
{
    public class UserController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserController> _logger;

        public UserController(IMediator mediator, ILogger<UserController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }


        // POST:api/User/login
        [AllowAnonymous]
        [HttpPost("/api/Users/login")]
        public async Task<IActionResult> Login([FromBody] Application.Users.Commands.LoginRequest request)
        {

            var response = await _mediator.Send(request);
            return Ok(response);
        }

        // POST: api/User/register
        [HttpPost("api/Users/register")]
        public async Task<IActionResult> Register([FromBody] Application.Users.Commands.RegisterRequest request)

        {
            _logger.LogInformation("------- Register request received.");
            var response = await _mediator.Send(request);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);

        }

        // GET: api/User/{id}
        [HttpGet("api/Users/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var response = await _mediator.Send(new GetByIdRequest(id));
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

    }
}
