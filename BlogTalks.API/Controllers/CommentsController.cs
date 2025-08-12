using BlogTalks.API.DTOs;
using BlogTalks.Application.Comments.Commands;
using BlogTalks.Application.Comments.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlogTalks.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CommentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //  GET: api/<CommentsController>
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var comments = await _mediator.Send(new GetRequest());
            return Ok(comments);
        }

        // GET api/<CommentsController>/5
        [HttpGet("{id}", Name = "GetCommentById")]
        public async Task<ActionResult> Get([FromRoute] GetByIdRequest request)
        {
            var comment = await _mediator.Send(request);

            if (comment == null)
                return NotFound();

            return Ok(comment);
        }

        // POST api/<CommentsController>
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] AddComand request)
        {
            var response = await _mediator.Send(request);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        //PUT api/<CommentsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute]int id, [FromBody] UpdateRequest request)
        {
            var response = await _mediator.Send(new UpdateRequest(id,request.Text));

            if (response == null)
                return NotFound();

            return NoContent();
        }

        // DELETE api/<CommentsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _mediator.Send(new DeleteRequest(id));

            if (response == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        //[HttpGet("blogPosts/{blogPostId}/comments")]
        [HttpGet("blogPosts/{blogPostId}/comments")]
        public async Task<IActionResult> GetByBlogPostId(int blogPostId)
        {
            var request = new GetByBlogPostIdRequest(blogPostId);
            var response = await _mediator.Send(request);

            return Ok(response);
        }

    }
}
