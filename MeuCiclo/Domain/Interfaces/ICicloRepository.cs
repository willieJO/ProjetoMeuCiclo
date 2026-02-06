namespace MeuCiclo.Domain.Interfaces
{
    public interface ICicloRepository
    {
        Task AddAsync(Ciclo ciclo);
        Task<IEnumerable<Ciclo>> GetAllAsync();
    }
}
