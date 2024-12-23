using MediatorExample.Application.Extensions;
using MediatorExample.Application.Services;
using MediatR;

namespace MediatorExample.Application.Features.Auth.ChangePasswd;

public class ChangePasswdHandler(IUserRepository userRepository) : IRequestHandler<ChangePasswdQuery, bool>
{
    public Task<bool> Handle(ChangePasswdQuery request, CancellationToken cancellationToken)
    {
        // get user from token gatekeeper stack
        var email = request.Token.JwtGetUserEmail();
        if (email == null) return Task.FromResult(false);
        var user = userRepository.GetUserByEmail(email);
        if (user == null) return Task.FromResult(false);

        var result = userRepository.ChangeUserPasswd(email, request.NewPassword.Hash());
        return Task.FromResult(result);
    }
}