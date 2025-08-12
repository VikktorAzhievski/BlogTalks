using BlogTalks.Application.Users.Commands;
using BlogTalks.Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace BlogTalks.API.Controllers
{
    public class UserController : Controller
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator) => _mediator = mediator;

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
