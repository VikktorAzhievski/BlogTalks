using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.Comments.Commands
{
    public record AddComand(string Text,int BlogPostId) : IRequest<AddResponse>;
}
