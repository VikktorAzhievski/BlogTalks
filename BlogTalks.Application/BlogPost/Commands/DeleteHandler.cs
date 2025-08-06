using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Repositories;
using BlogTalks.Infrastructure;
using MediatR;

namespace BlogTalks.Application.BlogPost.Commands
{
    public class DeleteHandler : IRequestHandler<DeleteRequest, DeleteResponse>
    {
        private readonly IRepository<BlogTalks.Domain.Entities.BlogPost> _blogPostRepository;

        public DeleteHandler(IRepository<BlogTalks.Domain.Entities.BlogPost> blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
        }

        public async Task<DeleteResponse> Handle(DeleteRequest request, CancellationToken cancellationToken)
        {
            var blogPost = _blogPostRepository.GetById(request.Id);
            if (blogPost == null)
            {
                return null;
            }
            _blogPostRepository.Delete(blogPost);
            return new DeleteResponse();
        }
    }
}
