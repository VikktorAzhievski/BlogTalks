using BlogTalks.Infrastructure;
using BlogTalks.Application.Comments.Queries;
using MediatR;

namespace BlogTalks.Application.BlogPost.Commands
{
    public class UpdateHandler : IRequestHandler<UpdateRequest, UpdateResponse>
    {
        private readonly FakeDataStoreBlog _dataStore;

        public UpdateHandler(FakeDataStoreBlog dataStore)
        {
            _dataStore = dataStore;
        }

        public async Task<UpdateResponse> Handle(UpdateRequest request, CancellationToken cancellationToken)
        {
            var blogPost = await _dataStore.GetBlogPostByIdAsync(request.BlogPost.Id);

            if (blogPost == null)
            {
                return null!;
            }

            blogPost.Title = request.BlogPost.Title;
            blogPost.Text = request.BlogPost.Text;
            blogPost.Timestamp = DateTime.UtcNow;

            await _dataStore.UpdateBlogPostAsync(blogPost);


            return new UpdateResponse
            {
                Id = blogPost.id,
                Title = blogPost.Title,
                Text = blogPost.Text,
                CreatedBy = blogPost.CreatedBy,
                Timestamp = blogPost.Timestamp,
                Tags = blogPost.Tags,
                Comments = blogPost.Comments.Select(c => new BlogTalks.Application.Comments.Queries.GetResponse
                {
                    Id = c.Id,
                    Text = c.Text,
                    CreatedBy = c.CreatedBy,
                    CreatedAt = c.CreatedAt
                }).ToList()
            };
        }
    }
}
