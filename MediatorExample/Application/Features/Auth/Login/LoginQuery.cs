using MediatorExample.Application.Domain;
using MediatR;

namespace MediatorExample.Application.Features.Auth.Login;

public record LoginQuery(User User): IRequest<string>;