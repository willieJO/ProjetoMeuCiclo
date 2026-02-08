using MediatR;
using MeuCiclo.Domain;
using MeuCiclo.Domain.Entities;

namespace MeuCiclo.Application.Queries
{
    public record GetCiclosQuery() : IRequest<IEnumerable<Ciclo>>;


}
