using MeuCiclo.Domain.Entities;

namespace MeuCiclo.Domain.Interfaces
{
    public interface ICicloRepository
    {
        Task AddAsync(Ciclo ciclo);
        Task UpdateAsync(Ciclo ciclo);
        Task DeleteAsync(Ciclo ciclo);
        Task<Ciclo?> GetByIdAsync(Guid id);
        Task<IEnumerable<Ciclo>> GetAllAsync();
    }
}
