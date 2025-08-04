using BlogTalks.Application.Comments.Commands;
using MediatR;

namespace BlogTalks.Application.BlogPost.Commands
{
    public record UpdateRequest(int Id, UpdateResponse BlogPost) : IRequest<UpdateResponse>;
}
