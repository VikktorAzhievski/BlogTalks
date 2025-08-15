using BlogTalks.API.DTOs;
using BlogTalks.Application;
using BlogTalks.Application.BlogPost.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace BlogTalks.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<BlogPostsController> _logger;
        public BlogPostsController(IMediator mediator, ILogger<BlogPostsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        // GET: api/<BlogPostsController>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] string? searchWord,
            [FromQuery] string? tag,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var response = await _mediator.Send(new GetRequest(searchWord, tag, pageNumber, pageSize));
            return Ok(response);
        }
        // GET api/<BlogPostsController>/5
        [HttpGet("{id}", Name = "GetBlogPostById")]
        public async Task<IActionResult> GetById(int id)
        {
            _logger.LogInformation("Fetching blog by id.");
            try
            {
                var request = new GetByIdRequest(id);
                var blogPost = await _mediator.Send(request);
                return Ok(blogPost);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        // POST api/<BlogPostsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddCommand command)
        {
            _logger.LogInformation("Adding a new blog");
            var created = await _mediator.Send(command);
            return CreatedAtRoute("GetBlogPostById", new { id = created.Id }, created);
        }

        // PUT api/<BlogPostsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateRequest request)
        {
            var blogpost = await _mediator.Send(new UpdateRequest(id, request.Title, request.Text, request.Tags));

            if (blogpost == null)
            {
                return NotFound($"Blog post with ID {id} not found.");
            }

            return Ok(blogpost);
        }

        // DELETE api/<BlogPostsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Deleting blog");
            var response = await _mediator.Send(new DeleteRequest(id));

            if (response == null)
                return NotFound();

            return NoContent();
        }


    }
}
