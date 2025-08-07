using MediatR;
using BlogTalks.Domain.Repositories;
using BlogTalks.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace BlogTalks.Application.Comments.Commands
{
    public class DeleteHandler : IRequestHandler<DeleteRequest, DeleteResponse>
    {
        private readonly IRepository<Comment> _commentRepository;
        public DeleteHandler(IRepository<Comment> commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<DeleteResponse?> Handle(DeleteRequest request, CancellationToken cancellationToken)
        {
            var comment = _commentRepository.GetById(request.Id);

            if (comment == null)
            {
                return null;
            }

            _commentRepository.Delete(comment);

            return new DeleteResponse();
        }

    }
}
