using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogTalks.Application.Users.Commands
{
    public class LoginResponse
    {
        public string Token { get; }
        public string RefreshToken { get; }
        public string Message { get; } 

        public LoginResponse(string Token, string RefreshToken, string Message)
        {
            this.Token = Token;
            this.RefreshToken = RefreshToken;
            this.Message = Message;
        }
    }


}
