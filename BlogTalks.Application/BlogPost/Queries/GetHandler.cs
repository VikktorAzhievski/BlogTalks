using BlogTalks.Domain.Entities;
using BlogTalks.Infrastructure;
using MediatR;

namespace BlogTalks.Application.BlogPost.Commands
{
    public class GetHandler : IRequestHandler<GetRequest, IEnumerable<GetResponse>>
    {
        private readonly FakeDataStoreBlog _dataStore;

        public GetHandler(FakeDataStoreBlog dataStore)
        {
            _dataStore = dataStore;
        }

        public async Task<IEnumerable<GetResponse>> Handle(GetRequest request, CancellationToken cancellationToken)
        {
            var blogPosts = await _dataStore.GetAllBlogPostsAsync();

            return blogPosts.Select(bp => new GetResponse
            {
                Id = bp.id,
                Title = bp.Title,
                Text = bp.Text,
                Tags = bp.Tags,
                Timestamp = bp.Timestamp,
                CreatedBy = bp.CreatedBy,
                Comments = bp.Comments
            });
        }
    }
}
