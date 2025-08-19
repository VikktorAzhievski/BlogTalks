using BlogTalks.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BlogTalks.Application.BlogPost.Commands
{
    public class UpdateHandler : IRequestHandler<UpdateRequest, UpdateResponse>
    {
        private readonly IRepository<BlogTalks.Domain.Entities.BlogPost> _blogPostRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateHandler(IRepository<BlogTalks.Domain.Entities.BlogPost> blogPostRepository, IHttpContextAccessor httpContextAccessor)
        {
            _blogPostRepository = blogPostRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UpdateResponse> Handle(UpdateRequest request, CancellationToken cancellationToken)
        {
            var blogPost = _blogPostRepository.GetById(request.Id);

            if (blogPost == null)
            {
                return null!;
            }

            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value;
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
            int userId = int.Parse(userIdClaim);

            if (blogPost.CreatedBy != userId)
            {
                throw new UnauthorizedAccessException("You do not have permission to update this blog post.");
            }

            blogPost.Title = request.Title;
            blogPost.Text = request.Text;
            blogPost.CreatedAt = DateTime.UtcNow;
            blogPost.Tags = request.Tags;

            _blogPostRepository.Update(blogPost);

            return Task.FromResult(new UpdateResponse
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                Text = blogPost.Text,
                Timestamp = DateTime.UtcNow,
                CreatedBy = blogPost.CreatedBy,
                Tags = blogPost.Tags
            }).Result;



        }
    }
}
