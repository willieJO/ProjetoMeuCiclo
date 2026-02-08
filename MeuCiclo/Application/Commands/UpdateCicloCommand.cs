using MediatR;

namespace MeuCiclo.Application.Commands
{
    public record UpdateCicloCommand(Guid Id, DateTime Data, string Fluxo) : IRequest;

}
