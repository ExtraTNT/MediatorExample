using MediatorExample.Application.Extensions;
using MediatorExample.Application.Services;
using MediatR;

namespace MediatorExample.Application.Features.Auth.Register;

public class RegisterHandler(IUserRepository userRepository) : IRequestHandler<RegisterQuery, bool>
{
    public Task<bool> Handle(RegisterQuery request, CancellationToken cancellationToken)
    {
        var password = request.User.Password;
        var hash = password.Hash();
        return Task.FromResult(userRepository.AddUser(request.User.Email, hash));
    }
}