using BlogTalks.Infrastructure;
using MediatR;

namespace BlogTalks.Application.BlogPost.Commands
{
    public class GetByIdHandler : IRequestHandler<GetByIdRequest, GetByIdResponse>
    {
        private readonly FakeDataStoreBlog _dataStore;

        public GetByIdHandler(FakeDataStoreBlog dataStore)
        {
            _dataStore = dataStore;
        }

        public async Task<GetByIdResponse> Handle(GetByIdRequest request, CancellationToken cancellationToken)
        {
            var post = await _dataStore.GetBlogPostByIdAsync(request.id);

            if (post == null)
                throw new NotFoundException($"Blog post with ID {request.id} not found.");

            return new GetByIdResponse
            {
                Id = post.id,
                Title = post.Title,
                Text = post.Text,
                Tags = post.Tags,
                Timestamp = post.Timestamp,
                CreatedBy = post.CreatedBy,
                Comments = post.Comments
            };
        }
    }
}
