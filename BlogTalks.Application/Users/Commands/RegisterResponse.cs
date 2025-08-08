using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.Users.Commands
{
    public record RegisterResponse(int Id, string Username, string Email, string? ErrorMessage = null);

}
