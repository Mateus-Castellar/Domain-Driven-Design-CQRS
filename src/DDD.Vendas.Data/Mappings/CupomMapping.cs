using DDD.Vendas.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DDD.Vendas.Data.Mappings
{
    public class CupomMapping : IEntityTypeConfiguration<Cupom>
    {
        public void Configure(EntityTypeBuilder<Cupom> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Codigo)
                .IsRequired()
                .HasColumnType("varchar(100)");

            builder.HasMany(c => c.Pedidos)
                .WithOne(c => c.Cupom)
                .HasForeignKey(c => c.CupomId);

            builder.ToTable("Cupons");
        }
    }
}