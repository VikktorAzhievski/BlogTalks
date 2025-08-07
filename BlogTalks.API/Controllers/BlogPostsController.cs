using BlogTalks.API.DTOs;
using BlogTalks.Application;
using BlogTalks.Application.BlogPost.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace BlogTalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public BlogPostsController(IMediator mediator) => _mediator = mediator;

        // GET: api/<BlogPostsController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var blogPosts = await _mediator.Send(new GetRequest());
            return Ok(blogPosts);
        }
        // GET api/<BlogPostsController>/5
        [HttpGet("{id}", Name = "GetBlogPostById")]
        public async Task<IActionResult> GetById(int id)
        {
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
            var created = await _mediator.Send(command);
            return CreatedAtRoute("GetBlogPostById", new { id = created.Id }, created);
        }

        // PUT api/<BlogPostsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UpdateRequest request)
        {
            var blogpost = await _mediator.Send(new UpdateRequest(id, request.Title,request.Text,request.Tags));

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
            var response = await _mediator.Send(new DeleteRequest(id));

            if (response == null)
                return NotFound();

            return NoContent();
        }

       
    }
}
