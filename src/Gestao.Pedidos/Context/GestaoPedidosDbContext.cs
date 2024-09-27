using Gestao.Pedidos.Recepcao;
using Gestao.Pedidos.Recepcao.EFMapping;
using Gestao.Pedidos.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Gestao.Pedidos.Context
{
    public class GestaoPedidosDbContext : DbContext
    {
        private readonly IMediator _mediator;
        public GestaoPedidosDbContext(IMediator mediator)
        {
            _mediator = mediator;
        }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<ItemPedido> ItemsPedido { get; set; }

        public DbSet<DescontoSazonal> DescontosSazonais { get; set; }
        public DbSet<DescontoQuantidade> DescontosQuantidade { get; set; }


        public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var sucesso = false;
            try
            {
                sucesso = await base.SaveChangesAsync(cancellationToken) > 0;
            }
            catch (Exception)
            {
                sucesso = false;

            }
            if (sucesso)
            {
                var domainEntities = ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.Events != null && x.Entity.Events.Any());

                var domainEvents = domainEntities
                    .SelectMany(x => x.Entity.Events)
                    .ToList();

                domainEntities.ToList()
                    .ForEach(entity => entity.Entity.LimparEventos());


                foreach (var evento in domainEvents)
                {
                    await _mediator.Publish(evento, cancellationToken);
                }
                //var tasks = domainEvents
                //    .Select(async (domainEvent) => {
                //        await mediator.Publish(domainEvent);
                //    });

                //await Task.WhenAll(tasks);
            }

            return sucesso;
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

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

        //void InserirProdutos()
        //{
        //    var context = new GestaoPedidosDbContext();

        //    var produto1 = new Produto(Guid.NewGuid().ToString(), 120.45m, 50, new DescontoQuantidade(5, 15));
        //    var produto2 = new Produto(Guid.NewGuid().ToString(), 90.56m, 50, new DescontoQuantidade(3, 10));

        //    context.Produtos.Add(produto1);
        //    context.Produtos.Add(produto2);

        //    context.SaveChanges();
        //}
    }
}
