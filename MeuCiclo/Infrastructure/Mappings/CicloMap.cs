using MeuCiclo.Domain;
using MeuCiclo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MeuCiclo.Infrastructure.Mappings
{
    public class CicloMap : IEntityTypeConfiguration<Ciclo>
    {
        public void Configure(EntityTypeBuilder<Ciclo> builder)
        {
            builder.ToTable("Ciclos");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Fluxo)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.Data)
                .IsRequired();
        }
    }
}
