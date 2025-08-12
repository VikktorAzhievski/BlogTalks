using BlogTalks.Application.BlogPost.Commands;
using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;

public class AddHandler : IRequestHandler<AddCommand, AddResponse>
{
    private readonly IRepository<BlogTalks.Domain.Entities.BlogPost> _blogPostRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public AddHandler(IRepository<BlogTalks.Domain.Entities.BlogPost> blogPostRepository, IHttpContextAccessor httpContextAccessor)
    {
        _blogPostRepository = blogPostRepository;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<AddResponse> Handle(AddCommand request, CancellationToken cancellationToken)
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("id")?.Value;

        if (userIdClaim == null)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        if (!int.TryParse(userIdClaim, out var userId))
        {
            throw new InvalidOperationException("Invalid user ID format.");
        }


        var blogPost = new BlogPost
        {
            Title = request.Title,
            Text = request.Text,
            Tags = request.Tags,
            CreatedBy = userId, 
            CreatedAt = DateTime.UtcNow
        };

        _blogPostRepository.Add(blogPost);

        return new AddResponse { Id = blogPost.Id };

    }
}
