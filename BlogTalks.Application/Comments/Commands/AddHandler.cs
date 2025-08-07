using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Repositories;
using MediatR;

namespace BlogTalks.Application.Comments.Commands
{
    public class AddCommentHandler : IRequestHandler<AddComand, AddResponse>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IBlogPostRepository _blogPostRepository;

        public AddCommentHandler(ICommentRepository commentRepository, IBlogPostRepository blogPostRepository)
        {
            _commentRepository = commentRepository;
            _blogPostRepository = blogPostRepository;
        }

        public async Task<AddResponse> Handle(AddComand request, CancellationToken cancellationToken)
        {
            var blogPost = _blogPostRepository.GetById(request.BlogPostId);
            if (blogPost == null)
                return null; 

            var comment = new Comment
            {
                BlogPostId = request.BlogPostId,
                Blog = blogPost,
                Text = request.Text,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = 5,
            };

            _commentRepository.Add(comment);

            return new AddResponse(comment.Id);
        }
    }
}
