using Gestao.Pedidos.Recepcao;
using Gestao.Pedidos.Recepcao.EFMapping;
using Gestao.Pedidos.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Gestao.Pedidos.Context
{
    public class GestaoPedidosDbContext : DbContext
    {
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItemsPedido { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ItemPedidoMapping());
            modelBuilder.ApplyConfiguration(new ProdutoMapping());
            modelBuilder.ApplyConfiguration(new PedidoMapping());

            modelBuilder.Ignore<DomainEvent>();

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost,11433;Database=gestao-pedidos;User Id=sa;Password=D3vP@ss!8;Integrated Security=false; Encrypt=false;");
        }
    }
}
