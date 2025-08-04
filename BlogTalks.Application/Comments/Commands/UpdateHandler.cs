using BlogTalks.Infrastructure;
using BlogTalks.Domain.Entities;
using MediatR;

namespace BlogTalks.Application.Comments.Commands
{
    public class UpdateHandler : IRequestHandler<UpdateRequest, UpdateResponse>
    {
        private readonly FakeDataStore _dataStore;

        public UpdateHandler(FakeDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        public async Task<UpdateResponse> Handle(UpdateRequest request, CancellationToken cancellationToken)
        {
            var comment = await _dataStore.GetCommentById(request.Id);

            if (comment == null)
                return null!;

            comment.Text = request.Text;
            comment.CreatedBy = request.CreatedBy;
            comment.CreatedAt = DateTime.UtcNow;

            await Task.CompletedTask;

            return new UpdateResponse
            {
                Id = comment.Id,
                Text = comment.Text,
                CreatedBy = comment.CreatedBy,
                CreatedAt = comment.CreatedAt
            };
        }
    }
}
