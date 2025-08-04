using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace BlogTalks.Application.Comments.Commands
{
    public class DeleteHandler : IRequestHandler<DeleteCommand, DeleteResponse>
    {
        private readonly FakeDataStore _dataStore;

        public DeleteHandler(FakeDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public async Task<DeleteResponse> Handle(DeleteCommand request, CancellationToken cancellationToken)
        {
            var result = await _dataStore.DeleteCommentAsync(request.Id);

            if (!result)
            {
                return new DeleteResponse(false, "Comment not found or could not be deleted.");
            }

            return new DeleteResponse(true, "Comment deleted successfully.");
        }
    }
}
