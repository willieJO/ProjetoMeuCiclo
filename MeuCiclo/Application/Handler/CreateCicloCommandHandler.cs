using MediatR;
using MeuCiclo.Application.Commands;
using MeuCiclo.Domain;
using MeuCiclo.Domain.Interfaces;

namespace MeuCiclo.Application.Handler
{
    public class CreateCicloCommandHandler : IRequestHandler<CreateCicloCommand, Guid>
    {
        private readonly ICicloRepository _repository;

        public CreateCicloCommandHandler(ICicloRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateCicloCommand request, CancellationToken cancellationToken)
        {
            var ciclo = new Ciclo(request.Data, request.Fluxo);
            await _repository.AddAsync(ciclo);
            return ciclo.Id;
        }
    }
}
