using BlogTalks.API.Controllers;
using BlogTalks.API.DTOs;
using BlogTalks.Application;
using BlogTalks.Application.Comments.Commands;
using BlogTalks.Application.Comments.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlogTalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
       private static  List<CommentsDto> comments = new List<CommentsDto>
                {
                    new CommentsDto { Id = 1, Text = "First comment", CreatedAt = DateTime.Now, CreatedBy = 1 },
                    new CommentsDto { Id = 2, Text = "Second comment", CreatedAt = DateTime.Now, CreatedBy = 2 },
                   };
        private readonly IMediator _mediator;
        public CommentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //  GET: api/<CommentsController>
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
            try
            {
                var comment = await _mediator.Send(request);
                return Ok(comment);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred." });
            }
        }

        // POST api/<CommentsController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] AddCommand request)
        {
            var commentToReturn = await _mediator.Send(new AddCommand(request.Id, request.Text, request.Timestamp, request.CreatedAt, request.CreatedBy, request.BlogPostId));

            return CreatedAtRoute("GetCommentById", new { id = commentToReturn.Id }, commentToReturn);
        }

        // PUT api/<CommentsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateRequest request)
        {
            if (id != request.Id)
                return BadRequest("ID mismatch");

            var response = await _mediator.Send(request);

            if (response == null)
                return NotFound("Comment not found.");

            return NoContent();
        }


        // DELETE api/<CommentsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _mediator.Send(new DeleteCommand(id));

            if (!response.IsSuccess)
                return NotFound(response.Message);

            return NoContent();
        }

        [HttpGet("blogPosts/{blogPostId}/comments")]
        public async Task<IActionResult> GetByBlogPostId(int blogPostId)
        {
            var request = new GetByBlogPostIdRequest(blogPostId);
            var response = await _mediator.Send(request);

            return Ok(response);
        }

    }
}
