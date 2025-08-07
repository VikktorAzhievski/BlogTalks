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
    public class GetByIdHandler : IRequestHandler<GetByIdRequest, GetByIdResponse>
    {
        private readonly IRepository<Comment> _commentRepository;

        public GetByIdHandler(IRepository<Comment> commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public Task<GetByIdResponse?> Handle(GetByIdRequest request, CancellationToken cancellationToken)
        {
            var comment = _commentRepository.GetById(request.id);

            if (comment == null)
            {
                return Task.FromResult<GetByIdResponse?>(null);
            }

            var response = new GetByIdResponse
            {
                Id = comment.Id,
                Text = comment.Text,
                CreatedAt = comment.CreatedAt,
                CreatedBy = comment.CreatedBy,
                BlogPostId = comment.BlogPostId
            };

            return Task.FromResult(response);
        }

    }
}