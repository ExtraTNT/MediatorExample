using MediatorExample.Application.Extensions;
using MediatorExample.Application.Services;
using MediatR;

namespace MediatorExample.Application.Features.Auth.Login;

public class LoginHandler(IUserRepository userRepository) : IRequestHandler<LoginQuery, string>
{
    public Task<string> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = userRepository.GetUserByEmail(request.User.Email);
        if (user == null || !request.User.Password.Check(user.Password))
        {
            return Task.FromResult("");
        }

        return Task.FromResult(user.GenerateToken());
    }
}