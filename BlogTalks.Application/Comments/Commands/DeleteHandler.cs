using MediatR;
using BlogTalks.Domain.Repositories;
using BlogTalks.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BlogTalks.Application.Comments.Commands
{
    public class DeleteHandler : IRequestHandler<DeleteRequest, DeleteResponse>
    {
        private readonly IRepository<Comment> _commentRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DeleteHandler(IRepository<Comment> commentRepository, IHttpContextAccessor httpContextAccessor)
        {
            _commentRepository = commentRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<DeleteResponse?> Handle(DeleteRequest request, CancellationToken cancellationToken)
        {
            var comment = _commentRepository.GetById(request.Id);

            if (comment == null)
            {
                return null;
            }
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value;
            if (!int.TryParse(userIdClaim, out int currentUserId))
            {
                return null;
            }
            if (comment.CreatedBy != currentUserId)
            {
                return null;
            }
            _commentRepository.Delete(comment);

            return new DeleteResponse();
        }

    }
}
