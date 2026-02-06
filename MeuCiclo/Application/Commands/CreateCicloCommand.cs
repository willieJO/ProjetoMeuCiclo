using MediatR;

namespace MeuCiclo.Application.Commands
{
    public record CreateCicloCommand(DateTime Data, string Fluxo) : IRequest<Guid>;

}
