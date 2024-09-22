using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gestao.Pedidos.Recepcao.EFMapping;

public sealed class ProdutoMapping : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Codigo)
              .IsRequired();

        builder.Property(p => p.Descricao)
              .IsRequired()
              .HasMaxLength(200);

        builder.Property(p => p.PrecoUnitario)
              .HasColumnType("decimal(18,2)");

        builder.Property(p => p.QuantidadeEstoque)
              .IsRequired();

        // Mapeamento para a relação com Desconto usando uma tabela de junção com discriminador
        builder.HasOne(p => p.Desconto)
              .WithMany()
              .HasForeignKey("DescontoId");  // Chave estrangeira para o desconto

        // Opcional: Nome da tabela
        builder.ToTable("Produtos");
    }
}

