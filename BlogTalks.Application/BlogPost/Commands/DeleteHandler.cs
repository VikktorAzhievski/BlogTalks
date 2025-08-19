using BlogTalks.Domain.Exceptions;
using BlogTalks.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net;

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
                throw new BlogTalksException($"Blog post with id {request.Id} not found", HttpStatusCode.NotFound);
            }

            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value;
            if (!int.TryParse(userIdClaim, out int currentUserId))
            {
                throw new BlogTalksException("User not authenticated", HttpStatusCode.Unauthorized);
            }

            if (blogPost.CreatedBy != currentUserId)
            {
                throw new BlogTalksException("You are not authorized to delete this blog post", HttpStatusCode.Forbidden);
            }

            _blogPostRepository.Delete(blogPost);
            return new DeleteResponse();
        }

    }
}
