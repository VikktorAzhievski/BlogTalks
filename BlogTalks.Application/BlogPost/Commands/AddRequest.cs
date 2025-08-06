using BlogTalks.Application.Comments.Commands;
using MediatR;
using MediatR;
using System.Collections.Generic;

namespace BlogTalks.Application.BlogPost.Commands
{
    public record AddCommand(string Text, string Title, List<string> Tags) : IRequest<AddResponse>;
}
