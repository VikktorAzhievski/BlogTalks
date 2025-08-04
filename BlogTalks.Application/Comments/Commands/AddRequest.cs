using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.Comments.Commands
{
    public record AddCommand(int Id, string Text, DateTime Timestamp, DateTime CreatedAt, int CreatedBy, int BlogPostId) : IRequest<AddResponse>;
}
