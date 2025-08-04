using BlogTalks.Infrastructure;
using MediatR;

namespace BlogTalks.Application.BlogPost.Commands
{
    public class DeleteHandler : IRequestHandler<DeleteRequest, DeleteResponse>
    {
        private readonly FakeDataStoreBlog _dataStore;

        public DeleteHandler(FakeDataStoreBlog dataStore)
        {
            _dataStore = dataStore;
        }

        public async Task<DeleteResponse> Handle(DeleteRequest request, CancellationToken cancellationToken)
        {
            var deleted = await _dataStore.DeleteBlogPostAsync(request.Id);

            return new DeleteResponse(
                deleted,
                deleted ? "Blog post deleted successfully." : "Blog post not found."
            );
        }
    }
}
