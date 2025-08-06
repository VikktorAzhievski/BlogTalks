using BlogTalks.Application.Comments.Commands;
using BlogTalks.Domain.Entities;
using MediatR;
using System.Text.Json.Serialization;

namespace BlogTalks.Application.BlogPost.Commands
{
    public record UpdateRequest([property:JsonIgnore]int Id, 
        string Title,
        string Text,
        List<string> Tags
       )  : IRequest<UpdateResponse>;
}
