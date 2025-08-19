using BlogTalks.Application.BlogPost.Commands;
using BlogTalks.Application.BlogPost.Responses;
using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace BlogTalks.Tests.UnitTests.BlogPosts
{
    public class GetHandlerTests
    {
        private readonly Mock<IBlogPostRepository> _blogPostRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly GetHandler _handler;

        public GetHandlerTests()
        {
            _blogPostRepositoryMock = new Mock<IBlogPostRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _handler = new GetHandler(_blogPostRepositoryMock.Object, _userRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WhenBlogPostsExist_ReturnsGetResponse()
        {
            // Arrange
            var blogPosts = new List<BlogPost>
            {
                new BlogPost { Id = 1, Title = "Title 1", Text = "Text 1", Tags = new List<string>{"tag1"}, CreatedBy = 5 },
                new BlogPost { Id = 2, Title = "Title 2", Text = "Text 2", Tags = new List<string>{"tag2"}, CreatedBy = 6 }
            };
            var users = new List<User>
            {
                new User { Id = 5, Username = "User5" },
                new User { Id = 6, Username = "User6" }
            };

            _blogPostRepositoryMock
                .Setup(r => r.GetPagedAsync(1, 10, null, null))
                .ReturnsAsync((blogPosts.Count, blogPosts));

            _userRepositoryMock
                .Setup(r => r.GetUsersByIds(It.IsAny<List<int>>()))
                .Returns(users);

            var request = new GetRequest(SearchWord: null, Tag: null);

            var result = await _handler.Handle(request, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(2, result.BlogPosts.Count);
            Assert.Equal("User5", result.BlogPosts.First(bp => bp.Id == 1).CreatorName);
            Assert.Equal("User6", result.BlogPosts.First(bp => bp.Id == 2).CreatorName);
        }

        [Fact]
        public async Task Handle_WhenNoBlogPostsExist_ReturnsEmptyResponse()
        {
            // Arrange
            var emptyList = new List<BlogPost>();

            _blogPostRepositoryMock
                .Setup(r => r.GetPagedAsync(1, 10, null, null))
                .ReturnsAsync((0, emptyList));

            _userRepositoryMock
                .Setup(r => r.GetUsersByIds(It.IsAny<List<int>>()))
                .Returns(new List<User>());

            var request = new GetRequest(SearchWord: null, Tag: null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.BlogPosts);
            Assert.Equal(0, result.Metadata.TotalCount);
            Assert.Equal(10, result.Metadata.PageSize);
            Assert.Equal(1, result.Metadata.PageNumber);
        }
    }
}