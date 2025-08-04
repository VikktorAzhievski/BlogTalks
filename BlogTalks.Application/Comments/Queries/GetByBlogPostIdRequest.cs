using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.Comments.Queries
{
    public class GetByBlogPostIdRequest : IRequest<IEnumerable<GetByBlogPostIdResponse>>
    {
        public int BlogPostId { get; set; }

        public GetByBlogPostIdRequest (int blogPostId)
        {
            BlogPostId = blogPostId;
        }
    }
}
