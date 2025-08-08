using BlogTalks.Domain.Entities;
using BlogTalks.Domain.Repositories;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace BlogTalks.Application.Users.Commands
{
    public class RegisterHandler : IRequestHandler<RegisterRequest, RegisterResponse>
    {
        private readonly IUserRepository _userRepository;

        public RegisterHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<RegisterResponse> Handle(RegisterRequest request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.FindByEmail(request.Email);

            if (existingUser != null)
            {
                return new RegisterResponse(0, "", "", "User already exists with this email.");
            }

            var registeredUser = await _userRepository.RegisterAsync(request.Username, request.Password, request.Email);

            if (registeredUser == null)
            {
                return new RegisterResponse(0, "", "", "User registration failed.");
            }

            return new RegisterResponse(registeredUser.Id, registeredUser.Username, registeredUser.Email);
        }

    }
}
