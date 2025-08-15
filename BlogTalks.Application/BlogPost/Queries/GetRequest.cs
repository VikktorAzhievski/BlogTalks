using BlogTalks.Application.Comments.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.BlogPost.Commands
{
    public record GetRequest(
        string? SearchWord,
        string? Tag,
        int PageNumber = 1,
        int PageSize = 10
    ) : IRequest<GetResponse>;


}
