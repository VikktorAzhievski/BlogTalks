using BlogTalks.Domain.Entities;
using BlogTalks.Infrastructure;
using MediatR;

namespace BlogTalks.Application.BlogPost.Commands
{
    public class AddBlogPostHandler : IRequestHandler<AddCommand, AddResponse>
    {
        private readonly FakeDataStoreBlog _dataStore;

        public AddBlogPostHandler(FakeDataStoreBlog dataStore)
        {
            _dataStore = dataStore;
        }

        public async Task<AddResponse> Handle(AddCommand request, CancellationToken cancellationToken)
        {
            var blogPost = new Blog
            {
                id = _dataStore.GetNextBlogPostId(),
                Title = request.Title,
                Text = request.Text,
                Tags = request.Tags,
                Timestamp = DateTime.Now,
                CreatedBy = request.CreatedBy,
                Comments = new List<Comment>()
            };

            await _dataStore.AddBlogPostAsync(blogPost);

            return new AddResponse
            {
                Id = blogPost.id,
                Title = blogPost.Title,
                Text = blogPost.Text,
                Tags = blogPost.Tags,
                Timestamp = blogPost.Timestamp,
                CreatedBy = blogPost.CreatedBy
            };
        }
    }
}
