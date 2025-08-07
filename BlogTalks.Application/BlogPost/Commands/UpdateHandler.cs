using BlogTalks.Domain.Repositories;
using MediatR;

namespace BlogTalks.Application.BlogPost.Commands
{
    public class UpdateHandler : IRequestHandler<UpdateRequest, UpdateResponse>
    {
        private readonly IRepository<BlogTalks.Domain.Entities.BlogPost> _blogPostRepository;

        public UpdateHandler(IRepository<BlogTalks.Domain.Entities.BlogPost> blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
        }

        public async Task<UpdateResponse> Handle(UpdateRequest request, CancellationToken cancellationToken)
        {
            var blogPost = _blogPostRepository.GetById(request.Id);

            if (blogPost == null)
            {
                return null!;
            }

            blogPost.Title = request.Title;
            blogPost.Text = request.Text;
            blogPost.CreatedAt = DateTime.UtcNow;

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
