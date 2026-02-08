using MediatR;
using MeuCiclo.Domain.Interfaces;

namespace MeuCiclo.Application.Commands;

public class DeleteCicloCommandHandler : IRequestHandler<DeleteCicloCommand>
{
    private readonly ICicloRepository _repo;

    public DeleteCicloCommandHandler(ICicloRepository repo)
    {
        _repo = repo;
    }

    public async Task Handle(DeleteCicloCommand request, CancellationToken cancellationToken)
    {
        var ciclo = await _repo.GetByIdAsync(request.Id);
        if (ciclo == null)
            throw new Exception("Ciclo não encontrado");

        await _repo.DeleteAsync(ciclo);
    }
}
