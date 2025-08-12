using BlogTalks.Application.Models;

namespace BlogTalks.Application.Abstractions
{
    public interface IAuthService
    {
        string CreateToken(JwtUserModel user);
    }
}

