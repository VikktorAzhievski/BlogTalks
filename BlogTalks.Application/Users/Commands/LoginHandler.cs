using BlogTalks.Application.Abstractions;
using BlogTalks.Application.Models;
using BlogTalks.Application.Users.Commands;
using BlogTalks.Domain.Repositories;
using MediatR;

namespace BlogTalks.Application.User.Commands;

public class LoginHandler : IRequestHandler<LoginRequest, LoginResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IAuthService _authService;

    public LoginHandler(IUserRepository userRepository, IAuthService authService)
    {
        _userRepository = userRepository;
        _authService = authService;
    }

    public async Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
            throw new ArgumentException("Email is required.");

        if (string.IsNullOrWhiteSpace(request.Password))
            throw new ArgumentException("Password is required.");

        var user = await _userRepository.FindByEmail(request.Email);

        if (user == null || !PasswordHasher.VerifyPassword(request.Password, user.Password))
        {
            return new LoginResponse(
                Token: string.Empty,
                RefreshToken: string.Empty,
                Message: "Invalid email or password."
            );
        }

        var jwtUser = new JwtUserModel
        {
            Id = user.Id,
            Username = user.Username,
            Name = user.Username,
            Email = user.Email,
            Roles = new List<string>()
        };

        var token = _authService.CreateToken(jwtUser);

        return new LoginResponse(
            Token: token,
            RefreshToken: Guid.NewGuid().ToString(),
            Message: "Login successful."
        );
    }

}