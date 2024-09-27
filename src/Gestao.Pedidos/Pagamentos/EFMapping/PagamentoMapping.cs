using Gestao.Pedidos.Recepcao;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace Gestao.Pedidos.Pagamentos.EFMapping
{
    public class PagamentoMapping : IEntityTypeConfiguration<Pagamento>
    {
        public void Configure(EntityTypeBuilder<Pagamento> builder)
        {
            builder.HasKey(p=>p.Id);

            builder
            .HasOne(p => p.Pedido)
            .WithOne()
            .HasForeignKey<Pagamento>(p => p.PedidoId);

            builder
           .HasDiscriminator<string>("TipoPagamento")
           .HasValue<PagamentoPix>("Pix")
           .HasValue<PagamentoParcelado>("Parcelado");

        }
    }

    public class PagamentoPixMapping : IEntityTypeConfiguration<PagamentoPix>
    {
        public void Configure(EntityTypeBuilder<PagamentoPix> builder)
        {
            builder.Property(p => p.PorcentagemDesconto)
                .HasColumnName("PorcetagemDesconto");
        }
    }

    public class PagamentoParceladoMapping : IEntityTypeConfiguration<PagamentoParcelado>
    {
        public void Configure(EntityTypeBuilder<PagamentoParcelado> builder)
        {
            builder.Property(p => p.NumeroDeParcelas)
                .HasColumnName("PorcetagemDesconto");
        }
    }
}
