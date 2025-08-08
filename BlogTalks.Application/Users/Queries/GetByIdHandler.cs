using BlogTalks.Domain.Repositories;
using MediatR;



namespace BlogTalks.Application.Users.Queries
{
    public class GetUserByIdHandler : IRequestHandler<GetByIdRequest, GetByIdResponse>

    {
        private readonly IUserRepository _userRepository;
        public GetUserByIdHandler(IUserRepository userRepository)

        {

            _userRepository = userRepository;

        }

        public async Task<GetByIdResponse> Handle(GetByIdRequest request, CancellationToken cancellationToken)

        {
            // With this line:
            var user = _userRepository.GetById(request.Id);

            if (user == null)

                throw new Exception("User not found");

            return new GetByIdResponse(user.Id, user.Username, user.Email);

        }

    }

}
