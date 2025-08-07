using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Repositories;
using MediatR;

namespace BlogTalks.Application.Comments.Queries
{
    public class GetByBlogPostIdHandler : IRequestHandler<GetByBlogPostIdRequest, IEnumerable<GetByBlogPostIdResponse>>
    {
        private readonly IRepository<Comment> _commentRepository;

        public GetByBlogPostIdHandler(IRepository<Comment> commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public Task<IEnumerable<GetByBlogPostIdResponse>> Handle(GetByBlogPostIdRequest request, CancellationToken cancellationToken)
        {
            var comments = _commentRepository.GetAll().Where(c => c.BlogPostId == request.BlogPostId).ToList();

            var response = comments.Select(x => new GetByBlogPostIdResponse
            {
                Id = x.Id,
                Text = x.Text,
                CreatedAt = x.CreatedAt,
                CreatedBy = x.CreatedBy,
                BlogPostId = x.BlogPostId,
            });

            return Task.FromResult(response);
        }

    }
}
