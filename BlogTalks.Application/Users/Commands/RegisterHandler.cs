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
                return null;
            }

            var registeredUser = await _userRepository.RegisterAsync(request.Username, request.Password, request.Email);

            if (registeredUser == null)
            {
                return null;
            }

            return new RegisterResponse { Message = "Register successfull" };
        }

    }
}
