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

        public DbSet<DescontoSazonal> DescontosSazonais { get; set; }
        public DbSet<DescontoQuantidade> DescontosQuantidade { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ItemPedidoMapping());
            modelBuilder.ApplyConfiguration(new ProdutoMapping());
            modelBuilder.ApplyConfiguration(new PedidoMapping());
            modelBuilder.ApplyConfiguration(new DescontoMapping());
            modelBuilder.ApplyConfiguration(new DescontoQuantidadeMapping());
            modelBuilder.ApplyConfiguration(new DescontoSazonalMapping());

            modelBuilder.Ignore<DomainEvent>();

            //InserirProdutos();

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=localhost,11433;Database=gestao-pedidos-final;User Id=sa;Password=D3vP@ss!8;Integrated Security=false; Encrypt=false;");
        }

        void InserirProdutos()
        {
            var context = new GestaoPedidosDbContext();

            var produto1 = new Produto("Livro 1", 120.45m, 50, new DescontoQuantidade(5, 15));
            var produto2 = new Produto("Livro 2", 90.56m, 50, new DescontoQuantidade(3, 10));

            context.Produtos.Add(produto1);
            context.Produtos.Add(produto2);

            context.SaveChanges();
        }
    }
}
