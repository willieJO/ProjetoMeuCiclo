using MediatR;
using MeuCiclo.Domain.Interfaces;

namespace MeuCiclo.Application.Commands;

public class UpdateCicloCommandHandler : IRequestHandler<UpdateCicloCommand>
{
    private readonly ICicloRepository _repo;

    public UpdateCicloCommandHandler(ICicloRepository repo)
    {
        _repo = repo;
    }

    public async Task Handle(UpdateCicloCommand request, CancellationToken cancellationToken)
    {
        var ciclo = await _repo.GetByIdAsync(request.Id);
        if (ciclo == null)
            throw new Exception("Ciclo não encontrado");

        ciclo.Update(request.Data, request.Fluxo);
        await _repo.UpdateAsync(ciclo);
    }
}
