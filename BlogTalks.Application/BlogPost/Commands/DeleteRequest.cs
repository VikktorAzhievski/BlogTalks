using BlogTalks.Application.Comments.Commands;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BlogTalks.Application.BlogPost.Commands
{
    public record DeleteRequest(int Id) : IRequest<DeleteResponse>;
}
