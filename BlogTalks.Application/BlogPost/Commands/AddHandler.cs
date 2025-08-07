using BlogTalks.Application.BlogPost.Commands;
using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Repositories;
using MediatR;

public class AddHandler : IRequestHandler<AddCommand, AddResponse>
{
    private readonly IRepository<BlogTalks.Domain.Entities.BlogPost> _blogPostRepository;

    public AddHandler(IRepository<BlogTalks.Domain.Entities.BlogPost> blogPostRepository)
    {
        _blogPostRepository = blogPostRepository;
    }
    public async Task<AddResponse> Handle(AddCommand request, CancellationToken cancellationToken)
    {
        var blogPost = new BlogPost
        {
           
            Title = request.Title,
            Text = request.Text,
            Tags = request.Tags,
            CreatedBy = 5,
            CreatedAt = DateTime.UtcNow
        };

        _blogPostRepository.Add(blogPost);

        return new AddResponse { Id = blogPost.Id };

    }
}
