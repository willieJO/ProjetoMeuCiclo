using MediatR;
using MeuCiclo.Domain.Entities;
using MeuCiclo.Domain.Interfaces;

namespace MeuCiclo.Application.Commands;

public class CreateCicloCommandHandler : IRequestHandler<CreateCicloCommand, Guid>
{
    private readonly ICicloRepository _repo;

    public CreateCicloCommandHandler(ICicloRepository repo)
    {
        _repo = repo;
    }

    public async Task<Guid> Handle(CreateCicloCommand request, CancellationToken cancellationToken)
    {
        var ciclo = new Ciclo(request.Data, request.Fluxo);
        await _repo.AddAsync(ciclo);
        return ciclo.Id;
    }
}
