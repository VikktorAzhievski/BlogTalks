using BlogTalks.Application.BlogPost.Commands;
using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Exceptions;
using BlogTalks.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Net;
using System.Security.Claims;
using Xunit;

namespace BlogTalks.Tests.UnitTests.BlogPosts
{
    public class DeleteHandlerTests
    {
        private readonly Mock<IRepository<BlogPost>> _blogPostRepositoryMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly DeleteHandler _handler;

        public DeleteHandlerTests()
        {
            _blogPostRepositoryMock = new Mock<IRepository<BlogPost>>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _handler = new DeleteHandler(_blogPostRepositoryMock.Object, _httpContextAccessorMock.Object);
        }

        [Fact]
        public async Task Handle_UserIsCreator_DeletesBlogPost()
        {
            var blogPost = new BlogPost { Id = 1, CreatedBy = 1, Title = "Test", Text = "Test" };
            _blogPostRepositoryMock.Setup(r => r.GetById(1)).Returns(blogPost);

            var claims = new[] { new Claim("id", "1") };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(c => c.User).Returns(principal);
            _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContextMock.Object);

            var result = await _handler.Handle(new DeleteRequest(1), default);

            Assert.NotNull(result);
            _blogPostRepositoryMock.Verify(r => r.Delete(blogPost), Times.Once);
        }

        [Fact]
        public async Task Handle_UserIsNotCreator_ThrowsException()
        {
            var blogPost = new BlogPost { Id = 1, CreatedBy = 2, Title = "Test", Text = "Test" };
            _blogPostRepositoryMock.Setup(r => r.GetById(1)).Returns(blogPost);

            var claims = new[] { new Claim("id", "1") };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(c => c.User).Returns(principal);
            _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContextMock.Object);

            var exception = await Assert.ThrowsAsync<BlogTalksException>(() => _handler.Handle(new DeleteRequest(1), default));

            Assert.Equal("You are not authorized to delete this blog post", exception.Message);
            Assert.Equal(HttpStatusCode.Forbidden, exception.StatusCode);

            _blogPostRepositoryMock.Verify(r => r.Delete(It.IsAny<BlogPost>()), Times.Never);
        }

    }
}
