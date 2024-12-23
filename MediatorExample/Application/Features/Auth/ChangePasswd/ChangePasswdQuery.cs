using MediatR;

namespace MediatorExample.Application.Features.Auth.ChangePasswd;

public record ChangePasswdQuery(string Token, string NewPassword): IRequest<bool>;