using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Repositories;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlogTalks.Application.BlogPost.Commands
{
    public class GetHandler : IRequestHandler<GetRequest, IEnumerable<GetResponse>>
    {
        private readonly IRepository<BlogTalks.Domain.Entities.BlogPost> _blogPostRepository;

        public GetHandler(IRepository<BlogTalks.Domain.Entities.BlogPost> blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
        }

        public async Task<IEnumerable<GetResponse>> Handle(GetRequest request, CancellationToken cancellationToken)
        {
           
            var blogPosts = _blogPostRepository.GetAll();

            return blogPosts.Select(bp => new GetResponse
            {
                Id = bp.Id,
                Title = bp.Title,
                Text = bp.Text,
                Tags = bp.Tags,
                CreatedBy = bp.CreatedBy,
                Comments = bp.Comments
            });
        }
    }
}
