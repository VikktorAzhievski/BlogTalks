using MediatR;
using BlogTalks.Domain.Entities;
using BlogTalks.Infrastructure;

namespace BlogTalks.Application.Comments.Queries
{
    public class GetByBlogPostIdHandler : IRequestHandler<GetByBlogPostIdRequest, IEnumerable<GetByBlogPostIdResponse>>
    {
        private readonly FakeDataStore _dataStore;

        public GetByBlogPostIdHandler(FakeDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public async Task<IEnumerable<GetByBlogPostIdResponse>> Handle(GetByBlogPostIdRequest request, CancellationToken cancellationToken)
        {
            var comments = await _dataStore.GetCommentsByBlogPostId(request.BlogPostId);

            return comments.Select(x => new GetByBlogPostIdResponse
            {
                Id = x.Id,
                Text = x.Text,
                CreatedAt = x.CreatedAt,
                CreatedBy = x.CreatedBy,
                BlogPostId = x.BlogPostId,
            });
           
        }
    }
}
