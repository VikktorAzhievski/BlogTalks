﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.Users.Commands
{
    public record LoginRequest(string Email, string Password) : IRequest<LoginResponse>;
}
