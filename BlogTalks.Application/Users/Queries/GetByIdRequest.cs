using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.Users.Queries
{
    public record GetByIdRequest(int Id) : IRequest<GetByIdResponse>;
}
