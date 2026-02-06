using MediatR;
using MeuCiclo.Application.Queries;
using MeuCiclo.Domain;
using MeuCiclo.Domain.Interfaces;

namespace MeuCiclo.Application.Handler
{
    public class GetCiclosQueryHandler : IRequestHandler<GetCiclosQuery, IEnumerable<Ciclo>>
    {
        private readonly ICicloRepository _repository;

        public GetCiclosQueryHandler(ICicloRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Ciclo>> Handle(GetCiclosQuery request, CancellationToken cancellationToken)
            => await _repository.GetAllAsync();
    }
}
