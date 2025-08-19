using System.Security.Claims;
using BlogTalks.Application.BlogPost.Commands;
using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace BlogTalks.Tests.UnitTests.BlogPosts
{
    public class AddHandlerTests
    {
        private readonly Mock<IRepository<BlogPost>> _blogPostRepositoryMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly AddHandler _handler;

        public AddHandlerTests()
        {
            _blogPostRepositoryMock = new Mock<IRepository<BlogPost>>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _handler = new AddHandler(_blogPostRepositoryMock.Object, _httpContextAccessorMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsAddResponse()
        {
            var userId = "1";
            var claims = new[] { new Claim("id", userId) };
            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(c => c.User).Returns(principal);
            _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContextMock.Object);

            var command = new AddCommand("Test Text", "Test Title", new List<string> { "tag1", "tag2" });

            _blogPostRepositoryMock
                .Setup(r => r.Add(It.IsAny<BlogPost>()))
                .Callback<BlogPost>(b => b.Id = 123);

            var result = await _handler.Handle(command, default);

            Assert.NotNull(result);
            Assert.Equal(123, result.Id);
            _blogPostRepositoryMock.Verify(r => r.Add(It.Is<BlogPost>(b =>
                b.Title == command.Title &&
                b.Text == command.Text &&
                b.Tags.SequenceEqual(command.Tags)
            )), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_ThrowUnauthorized_WhenUserIsNotAuthenticated()
        {
            var command = new AddCommand("Test Text", "Test Title", new List<string> { "tag1" });
            _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(new DefaultHttpContext());

            var action = async () => await _handler.Handle(command, default);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(action);
        }

        //[Fact]
        //public async Task Handle_Should_ThrowException_WhenRepositoryFails()
        //{
        //    // Arrange
        //    var userId = "1";
        //    var claims = new[] { new Claim("id", userId) };
        //    var identity = new ClaimsIdentity(claims, "TestAuth");
        //    var principal = new ClaimsPrincipal(identity);

        //    var httpContextMock = new Mock<HttpContext>();
        //    httpContextMock.Setup(c => c.User).Returns(principal);
        //    _httpContextAccessorMock.Setup(a => a.HttpContext).Returns(httpContextMock.Object);

        //    var command = new AddCommand("Test Text", "Test Title", new List<string> { "tag1" });

        //    _blogPostRepositoryMock
        //        .Setup(r => r.Add(It.IsAny<BlogPost>()))
        //        .Throws(new Exception("Database failure"));

        //    // Act
        //    var action = async () => await _handler.Handle(command, default);

        //    // Assert
        //    await Assert.ThrowsAsync<Exception>(action);
        //    _blogPostRepositoryMock.Verify(r => r.Add(It.IsAny<BlogPost>()), Times.Once);
        //}
    }
}

