using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace BlogTalks.Application.Comments.Queries
{
    public class GetHandler : IRequestHandler<GetRequest, IEnumerable<GetResponse>>
    {
        private readonly FakeDataStore _dataStore;
 
        public GetHandler(FakeDataStore dataStore)
        {
            _dataStore = dataStore;
        }
 
        public async Task<IEnumerable<GetResponse>> Handle(GetRequest request, CancellationToken cancellationToken)
        {
            var comments = await _dataStore.GetAllComments();
 
            var response = comments.Select(c => new GetResponse
            {
                Id = c.Id,
                Text = c.Text,
                CreatedAt = c.CreatedAt,
                CreatedBy = c.CreatedBy,
                BlogPostId = c.BlogPostId
            });
 
            return response;
        }
 
    }
 
}