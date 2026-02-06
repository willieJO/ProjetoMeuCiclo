using MeuCiclo.Domain;
using Microsoft.EntityFrameworkCore;

namespace MeuCiclo.Infrastructure.Persistence
{
    public class MeuCicloDbContext : DbContext
    {
        public DbSet<Ciclo> Ciclos => Set<Ciclo>();

        public MeuCicloDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MeuCicloDbContext).Assembly);
        }
    }
}
