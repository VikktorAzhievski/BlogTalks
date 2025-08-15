using BlogTalks.Application.BlogPost.Responses;
using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Repositories;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlogTalks.Application.BlogPost.Commands
{
    public class GetHandler : IRequestHandler<GetRequest, GetResponse>
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly IUserRepository _userRepository;

        public GetHandler(IBlogPostRepository blogPostRepository, IUserRepository userRepository)
        {
            _blogPostRepository = blogPostRepository;
            _userRepository = userRepository;
        }

        public async Task<GetResponse> Handle(GetRequest request, CancellationToken cancellationToken)
        {
            //var totalCount = await _blogPostRepository.GetCountAsync(request.SearchWord, request.Tag);

            var (totalCount, blogPosts) = await _blogPostRepository.GetPagedAsync(
                request.PageNumber,
                request.PageSize,
                request.SearchWord,
                request.Tag
            );

            var userIds = blogPosts.Select(bp => bp.CreatedBy).Distinct().ToList();
            var users = _userRepository.GetUsersByIds(userIds);

            var userDict = users.ToDictionary(u => u.Id, u => u.Username);
            var blogPostModels = blogPosts.Select(bp => new BlogPostModel
            {
                Id = bp.Id,
                Title = bp.Title,
                Text = bp.Text,
                Tags = bp.Tags,
                CreatorName = userDict.GetValueOrDefault(bp.CreatedBy, string.Empty)
            }).ToList();


            var response = new GetResponse
            {
                BlogPosts = blogPostModels,
                Metadata = new Metadata
                {
                    TotalCount = totalCount,
                    PageSize = request.PageSize,
                    PageNumber = request.PageNumber,
                    TotalPages = (int)System.Math.Ceiling(totalCount / (double)request.PageSize)
                }
            };

            return response;
        }
    }
}
