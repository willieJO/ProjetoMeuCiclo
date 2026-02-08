using MeuCiclo.Domain.Entities;
using MeuCiclo.Domain.Interfaces;
using MeuCiclo.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace MeuCiclo.Infrastructure.Repositories;

public class CicloRepository : ICicloRepository
{
    private readonly MeuCicloDbContext _context;

    public CicloRepository(MeuCicloDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Ciclo ciclo)
    {
        _context.Ciclos.Add(ciclo);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Ciclo ciclo)
    {
        _context.Ciclos.Update(ciclo);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Ciclo ciclo)
    {
        _context.Ciclos.Remove(ciclo);
        await _context.SaveChangesAsync();
    }

    public async Task<Ciclo?> GetByIdAsync(Guid id)
        => await _context.Ciclos.FirstOrDefaultAsync(x => x.Id == id);

    public async Task<IEnumerable<Ciclo>> GetAllAsync()
        => await _context.Ciclos.AsNoTracking().ToListAsync();
}
