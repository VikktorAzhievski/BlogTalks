using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace BlogTalks.Application.Comments.Queries
{
    public class GetHandler : IRequestHandler<GetRequest, IEnumerable<GetResponse>>
    {
        private readonly IRepository<Comment> _commentRepository;

        public GetHandler(IRepository<Comment> commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public Task<IEnumerable<GetResponse>> Handle(GetRequest request, CancellationToken cancellationToken)
        {
            var comments = _commentRepository.GetAll();

            var response = comments.Select(c => new GetResponse
            {
                Id = c.Id,
                Text = c.Text,
                CreatedAt = c.CreatedAt,
                CreatedBy = c.CreatedBy,
                BlogPostId = c.BlogPostId
            });

            return Task.FromResult(response);
        }


    }

}