using BlogTalks.Application;
using BlogTalks.Application.BlogPost.Commands;
using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Exceptions;
using BlogTalks.Domain.Repositories;
using Moq;
using Xunit;

namespace BlogTalks.Tests.UnitTests.BlogPosts;

public class GetByIdHandlerTests
{
    private readonly Mock<IBlogPostRepository> _blogPostRepositoryMock;
    private readonly GetByIdHandler _handler;

    public GetByIdHandlerTests()
    {
        _blogPostRepositoryMock = new Mock<IBlogPostRepository>();
        _handler = new GetByIdHandler(_blogPostRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_BlogPostExists_ReturnsBlogResponse()
    {
        var expectedId = 1;
        var expectedBlogPost = new BlogPost
        {
            Id = expectedId,
            Title = "Test Title",
            Text = "Test Text",
            Tags = new List<string> { "tag1", "tag2" },
            CreatedBy = 5,
            CreatedAt = DateTime.Now
        };

        _blogPostRepositoryMock
            .Setup(repo => repo.GetById(expectedId))
            .Returns(expectedBlogPost);

        var query = new GetByIdRequest(expectedId);

        var result = await _handler.Handle(query, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(expectedId, result.Id);
        Assert.Equal(expectedBlogPost.Title, result.Title);
        Assert.Equal(expectedBlogPost.Text, result.Text);
    }

    [Fact]
    public async Task Handle_BlogPostDoesNotExist_ThrowsException()
    {
        var expectedId = 125;
        _blogPostRepositoryMock
            .Setup(repo => repo.GetById(expectedId))
            .Returns((BlogPost?)null);

        var query = new GetByIdRequest(expectedId);

        await Assert.ThrowsAsync<NotFoundException>(() =>
            _handler.Handle(query, CancellationToken.None));
    }

}
