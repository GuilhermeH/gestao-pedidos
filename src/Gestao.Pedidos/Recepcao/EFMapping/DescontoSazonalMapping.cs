using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gestao.Pedidos.Recepcao.EFMapping
{
    public sealed class DescontoSazonalMapping : IEntityTypeConfiguration<DescontoSazonal>
    {
        public void Configure(EntityTypeBuilder<DescontoSazonal> builder)
        {
            builder.Property(d => d.DataInicial)
                  .IsRequired();

            builder.Property(d => d.DataFinal)
                  .IsRequired();

            builder.Property(d => d.Porcentagem)
                  .HasColumnType("decimal(5,2)");
        }
    }
}
