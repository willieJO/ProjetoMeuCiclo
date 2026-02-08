using MediatR;
using MeuCiclo.Domain.Entities;
using MeuCiclo.Domain.Interfaces;

namespace MeuCiclo.Application.Queries;

public class GetCiclosQueryHandler : IRequestHandler<GetCiclosQuery, IEnumerable<Ciclo>>
{
    private readonly ICicloRepository _repo;

    public GetCiclosQueryHandler(ICicloRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<Ciclo>> Handle(GetCiclosQuery request, CancellationToken cancellationToken)
        => await _repo.GetAllAsync();
}
