using MediatR;

namespace MeuCiclo.Application.Commands;

public record DeleteCicloCommand(Guid Id) : IRequest;
