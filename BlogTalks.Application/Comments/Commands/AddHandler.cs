using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;

namespace BlogTalks.Application.Comments.Commands
{
    public class AddCommentHandler : IRequestHandler<AddComand, AddResponse>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IUserRepository _userRepository;
        public AddCommentHandler(ICommentRepository commentRepository, IBlogPostRepository blogPostRepository, IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory, IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _blogPostRepository = blogPostRepository;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _userRepository = userRepository;
        }

        public async Task<AddResponse> Handle(AddComand request, CancellationToken cancellationToken)
        {
            var blogPost = _blogPostRepository.GetById(request.BlogPostId);
            if (blogPost == null)
                return null;

            // Check if the user is authenticated
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value;
            if (userIdClaim == null)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            if (!int.TryParse(userIdClaim, out var userid))
            {
                throw new InvalidOperationException("Invalid user ID format.");
            }

            var httpClient = _httpClientFactory.CreateClient("EmailSenderApi");
            var blogpostCreator = _userRepository.GetById(blogPost.CreatedBy);
            var commentCreator = _userRepository.GetById(userid);

            int userId = int.Parse(userIdClaim);
            var comment = new Comment
            {
                BlogPostId = request.BlogPostId,
                Blog = blogPost,
                Text = request.Text,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userId
            };

            _commentRepository.Add(comment);
            var emailDto = new
            {
                From = commentCreator.Email,
                To = commentCreator.Email,
                Subject = "New Comment Added",
                Body = " Someone added a new comment to your blog post: " + request.Text
            };
            await httpClient.PostAsJsonAsync("/send", emailDto);
            return new AddResponse(comment.Id);
        }
    }
}
