using Xunit;
using BlogTalks.Application.BlogPost.Commands;
using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlogTalks.Tests.BlogTalksProjectTests.UnitTests.BlogPosts
{
    public class UpdateHandlerTests
    {
        private readonly Mock<IRepository<BlogPost>> _blogPostRepositoryMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly UpdateHandler _handler;

        public UpdateHandlerTests()
        {
            _blogPostRepositoryMock = new Mock<IRepository<BlogPost>>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _handler = new UpdateHandler(_blogPostRepositoryMock.Object, _httpContextAccessorMock.Object);
        }

        [Fact]
        public async Task Handle_Should_UpdateBlogPost_WhenUserIsCreator()
        {
            var blogPost = new BlogPost
            {
                Id = 1,
                CreatedBy = 1,
                Title = "Original Title",
                Text = "Original Text",
                Tags = new List<string> { "tag1", "tag2" }
            };
            _blogPostRepositoryMock.Setup(x => x.GetById(1)).Returns(blogPost);

            var claims = new List<Claim> { new Claim("id", "1") };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var user = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext { User = user };
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

            var request = new UpdateRequest(
                Id: 1,
                Title: "Updated Title",
                Text: "Updated Text",
                Tags: new List<string> { "tag3", "tag4" }
            );

            var result = await _handler.Handle(request, default);

            result.Should().NotBeNull();
            result.Id.Should().Be(blogPost.Id);
            result.Title.Should().Be("Updated Title");
            result.Text.Should().Be("Updated Text");
            result.Tags.Should().BeEquivalentTo(new List<string> { "tag3", "tag4" });
            _blogPostRepositoryMock.Verify(x => x.Update(It.IsAny<BlogPost>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ThrowUnauthorized_WhenUserIsNotCreator()
        {
            var blogPost = new BlogPost
            {
                Id = 1,
                CreatedBy = 2, 
                Title = "Original Title",
                Text = "Original Text"
            };
            _blogPostRepositoryMock.Setup(x => x.GetById(1)).Returns(blogPost);

            var claims = new List<Claim> { new Claim("id", "1") };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var user = new ClaimsPrincipal(identity);
            var httpContext = new DefaultHttpContext { User = user };
            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

            var request = new UpdateRequest(
                Id: 1,
                Title: "Updated Title",
                Text: "Updated Text",
                Tags: new List<string> { "tag3", "tag4" }
            );

            var action = async () => await _handler.Handle(request, default);

            await action.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("You do not have permission to update this blog post.");
            _blogPostRepositoryMock.Verify(x => x.Update(It.IsAny<BlogPost>()), Times.Never);
        }
    }
}
