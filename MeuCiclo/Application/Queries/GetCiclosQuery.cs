using MediatR;
using MeuCiclo.Domain;

namespace MeuCiclo.Application.Queries
{
    public record GetCiclosQuery() : IRequest<IEnumerable<Ciclo>>;

}
