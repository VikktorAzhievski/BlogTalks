using BlogTalks.Application.Comments.Commands;
using MediatR;
using MediatR;
using System.Collections.Generic;

namespace BlogTalks.Application.BlogPost.Commands
{
    public record AddCommand(
        string Title,
        string Text,
        List<string> Tags,
        int CreatedBy
    ) : IRequest<AddResponse>;
}
