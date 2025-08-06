using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Repositories;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using BlogTalks.Domain.Entities;

namespace BlogTalks.Application.BlogPost.Commands
{
    public class GetByIdHandler : IRequestHandler<GetByIdRequest, GetByIdResponse>
    {
        private readonly IRepository<BlogTalks.Domain.Entities.BlogPost> _blogPostRepository;

        public GetByIdHandler(IRepository<BlogTalks.Domain.Entities.BlogPost> blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
        }

        public async Task<GetByIdResponse> Handle(GetByIdRequest request, CancellationToken cancellationToken)
        {
            var post = _blogPostRepository.GetById(request.id);

            if (post == null)
                throw new NotFoundException($"Blog post with ID not found.");

            return new GetByIdResponse
            {
                Id = post.Id,
                Title = post.Title,
                Text = post.Text,
                CreatedBy = post.CreatedBy,
                Comments = post.Comments
            };
        }
    }
}
