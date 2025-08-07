using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlogTalks.Application.Comments.Commands
{
    public record UpdateRequest([property:JsonIgnore]int id,string Text) : IRequest<UpdateResponse>;


}
