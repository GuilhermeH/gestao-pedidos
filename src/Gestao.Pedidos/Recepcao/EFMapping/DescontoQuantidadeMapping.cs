using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gestao.Pedidos.Recepcao.EFMapping
{
    internal class DescontoQuantidadeMapping : IEntityTypeConfiguration<DescontoQuantidade>
    {
        public void Configure(EntityTypeBuilder<DescontoQuantidade> builder)
        {
            builder.Property(d => d.QuantidadeAplicavel)
                   .IsRequired();

            builder.Property(d => d.ValorEmReais)
                  .HasColumnType("decimal(18,2)");
        }
    }
}
