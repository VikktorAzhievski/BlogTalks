using MediatR;
using BlogTalks.Domain.Repositories;
using BlogTalks.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BlogTalks.Application.Comments.Commands
{
    public class UpdateHandler : IRequestHandler<UpdateRequest, UpdateResponse?>
    {
        private readonly IRepository<Comment> _commentRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateHandler(IRepository<Comment> commentRepository, IHttpContextAccessor httpContextAccessor)
        {
            _commentRepository = commentRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UpdateResponse> Handle(UpdateRequest request, CancellationToken cancellationToken)
        {

            var comment = _commentRepository.GetById(request.id);

            if (comment == null)
                return null;

            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value;
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
            if (!int.TryParse(userIdClaim, out var userid))
            {
                throw new InvalidOperationException("Invalid user ID format.");
            }
            int userId = int.Parse(userIdClaim);
            if (comment.CreatedBy != userId)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this comment.");
            }

            comment.Text = request.Text;
            comment.CreatedAt = DateTime.UtcNow; 

            _commentRepository.Update(comment);
            
            return new UpdateResponse
            {
                Id = comment.Id,
                Text = comment.Text,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = comment.CreatedBy,
                BlogPostId = comment.BlogPostId
            };
        }
    }
}
