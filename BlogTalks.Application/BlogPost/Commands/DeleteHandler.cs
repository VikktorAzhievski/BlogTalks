using BlogTalks.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BlogTalks.Application.BlogPost.Commands
{
    public class DeleteHandler : IRequestHandler<DeleteRequest, DeleteResponse>
    {
        private readonly IRepository<BlogTalks.Domain.Entities.BlogPost> _blogPostRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public DeleteHandler(IRepository<BlogTalks.Domain.Entities.BlogPost> blogPostRepository, IHttpContextAccessor httpContextAccessor)
        {
            _blogPostRepository = blogPostRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<DeleteResponse> Handle(DeleteRequest request, CancellationToken cancellationToken)
        {
            var blogPost = _blogPostRepository.GetById(request.Id);
            if (blogPost == null)
            {
                return null;
            }
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value;
            if (!int.TryParse(userIdClaim, out int currentUserId))
            {
                return null;
            }
            if (blogPost.CreatedBy != currentUserId)
            {
                return null;
            }

            _blogPostRepository.Delete(blogPost);
            return new DeleteResponse();
        }
    }
}
