using MediatR;
using BlogTalks.Domain.Repositories;
using BlogTalks.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace BlogTalks.Application.Comments.Commands
{
    public class UpdateHandler : IRequestHandler<UpdateRequest, UpdateResponse?>
    {
        private readonly IRepository<Comment> _commentRepository;

        public UpdateHandler(IRepository<Comment> commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<UpdateResponse> Handle(UpdateRequest request, CancellationToken cancellationToken)
        {

            var comment = _commentRepository.GetById(request.id);

            if (comment == null)
                return null;

            comment.Text = request.Text;
            comment.CreatedAt = DateTime.UtcNow; 

            _commentRepository.Update(comment);
            
            return new UpdateResponse
            {
                Id = comment.Id,
                Text = comment.Text,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = comment.CreatedBy,
                BlogPostId = comment.BlogPostId
            };
        }
    }
}
