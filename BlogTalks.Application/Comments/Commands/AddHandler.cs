using BlogTalks.Application.Abstractions;
using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.FeatureManagement;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BlogTalks.Application.Comments.Commands
{
    public class AddCommentHandler : IRequestHandler<AddComand, AddResponse>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IServiceProvider _serviceProvider;
        private readonly IFeatureManager _featureManager;
        private readonly ILogger<AddCommentHandler> _logger;


        public AddCommentHandler(
            ICommentRepository commentRepository,
            IBlogPostRepository blogPostRepository,
            IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor,
            IServiceProvider serviceProvider,
            IFeatureManager featureManager,
            ILogger<AddCommentHandler> logger)
        {
            _commentRepository = commentRepository;
            _blogPostRepository = blogPostRepository;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
            _serviceProvider = serviceProvider;
            _featureManager = featureManager;
            _logger = logger;
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
            var blogpostCreator = _userRepository.GetById(blogPost.CreatedBy);
            var commentCreator = _userRepository.GetById(userid);

            var comment = new Comment
            {
                BlogPostId = request.BlogPostId,
                Blog = blogPost,
                Text = request.Text,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userid
            };

            _commentRepository.Add(comment);

            var emailDto = new Contracts.EmailDto()
            {
                From = commentCreator.Email,
                To = blogpostCreator.Email, 
                Subject = "New Comment Added",
                Body = $"Someone added a new comment to your blog post: {request.Text}"
            };

            if (await _featureManager.IsEnabledAsync("EmailHttpSender"))
            {
                var service = _serviceProvider.GetRequiredKeyedService<IMessagingService>("MessagingHttpService");
                await service.Send(emailDto);
            }
            else if (await _featureManager.IsEnabledAsync("EmailRabbitMQSender"))
            {
                var service = _serviceProvider.GetRequiredKeyedService<IMessagingService>("MessagingServiceRabbitMQ");
                await service.Send(emailDto);
            }
            else
            {
                _logger.LogError("No email sender feature flag is enabled. Email will not be sent.");
            }


            return new AddResponse(comment.Id);
        }
    }
}
