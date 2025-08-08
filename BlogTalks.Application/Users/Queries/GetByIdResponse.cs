using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.Users.Queries
{
    public record GetByIdResponse(int Id, string Username, string Email);

}
