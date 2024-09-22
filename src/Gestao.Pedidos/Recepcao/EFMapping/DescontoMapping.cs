using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gestao.Pedidos.Recepcao.EFMapping
{
    public sealed class DescontoMapping : IEntityTypeConfiguration<Desconto>
    {
        public void Configure(EntityTypeBuilder<Desconto> builder)
        {
            builder.HasKey(d => d.Id);

            builder.HasDiscriminator<string>("TipoDesconto")
                  .HasValue<DescontoSazonal>("Sazonal")
                  .HasValue<DescontoQuantidade>("Quantidade");

            builder.Property(d => d.Id)
            .IsRequired();

            // Todos os descontos na mesma tabela
            builder.ToTable("Descontos"); 
        }
    }
}
