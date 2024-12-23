using MediatorExample.Application.Domain;
using MediatR;

namespace MediatorExample.Application.Features.Auth.Register;

public record RegisterQuery(User User): IRequest<bool>;