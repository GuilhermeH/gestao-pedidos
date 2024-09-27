using Gestao.Pedidos.Context;
using Gestao.Pedidos.Estoque.Eventos;
using Gestao.Pedidos.Estoque.Handler;
using Gestao.Pedidos.Pagamentos;
using Gestao.Pedidos.Pagamentos.Eventos;
using Gestao.Pedidos.Recepcao;
using Gestao.Pedidos.Recepcao.Eventos;
using Gestao.Pedidos.Recepcao.Handlers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

internal class Program
{
    
    private static async Task Main(string[] args)
    {
        // Criar o host para configurar o DI
        var host = CreateHostBuilder(args).Build();

        // Resolver e executar a classe principal
        var app = host.Services.GetRequiredService<GestaoApp>();
        await app.Run();
        Console.ReadKey();

        // var mediator = 
        // ConfigurarMediatR();

        // var _context = new GestaoPedidosDbContext();

        //var context = new GestaoPedidosDbContext(mediator);


    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    // Registrar os serviços aqui
                    services.AddTransient<GestaoApp>(); // Adicionar a classe principal
                    services.AddMediatR(Assembly.GetExecutingAssembly());
                    //services.AddMediatR(typeof(SepararPedidoHandler).Assembly);
                    //services.AddMediatR(typeof(AvisarEstoqueAbaixoHandler).Assembly);
                    //services.AddMediatR(typeof(DebitarEstoqueHandler).Assembly);
                    //services.AddMediatR(typeof(EnviarEmailAlteracaoEstadoPedidoHandler).Assembly);

                    services.AddScoped<INotificationHandler<AvisarClienteAlteracaoEstadoPedidoEvent>, EnviarEmailAlteracaoEstadoPedidoHandler>();
                    services.AddScoped<INotificationHandler<EnviarSeparacaoEstoqueEvent>, SepararPedidoHandler>();
                    services.AddScoped<INotificationHandler<DebitarEstoqueEvent>, DebitarEstoqueHandler>();
                    services.AddScoped<INotificationHandler<AvisarClienteAlteracaoEstadoPedidoEvent>, EnviarEmailAlteracaoEstadoPedidoHandler>();

                    services.AddScoped<PedidoRepository>();
                    services.AddScoped<ProdutoRepository>();
                    services.AddScoped<GestaoPedidosDbContext>();
                });


    static void ConfigurarMediatR()
    {
        var services = new ServiceCollection();

        // Registra os serviços do MediatR
        // services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddMediatR(typeof(Program));
        //services.AddMediatR(typeof(SepararPedidoHandler).Assembly);
        //services.AddMediatR(typeof(AvisarEstoqueAbaixoHandler).Assembly);
        //services.AddMediatR(typeof(DebitarEstoqueHandler).Assembly);
        //services.AddMediatR(typeof(EnviarEmailAlteracaoEstadoPedidoHandler).Assembly);

        //services.AddScoped<INotificationHandler<AvisarClienteAlteracaoEstadoPedidoEvent>, EnviarEmailAlteracaoEstadoPedidoHandler>();
        //services.AddScoped<INotificationHandler<EnviarSeparacaoEstoqueEvent>, SepararPedidoHandler>();
        //services.AddScoped<INotificationHandler<DebitarEstoqueEvent>, DebitarEstoqueHandler>();
        //services.AddScoped<INotificationHandler<AvisarClienteAlteracaoEstadoPedidoEvent>, EnviarEmailAlteracaoEstadoPedidoHandler>();
        services.AddScoped<GestaoPedidosDbContext>();
        services.AddScoped<PedidoRepository>();
        services.AddScoped<ProdutoRepository>();
        services.AddScoped<GestaoPedidosDbContext>();

        // Constrói o ServiceProvider
        var serviceProvider = services.BuildServiceProvider();

        // Resolve o MediatR (usado para enviar comandos e consultas)
        //var mediator = serviceProvider.GetRequiredService<IMediator>();





        // return mediator;
    }

    
}

public class GestaoApp
{
    private readonly ProdutoRepository _produtoRepository;
    private readonly PedidoRepository _pedidoRepository;
    public GestaoApp(ProdutoRepository produtoRepository, PedidoRepository pedidoRepository)
    {
        _produtoRepository = produtoRepository;
        _pedidoRepository = pedidoRepository;
    }

    public async Task Run()
    {
        Console.WriteLine("Informe um email válido: \n");
        var emailCLiente = Console.ReadLine();

        var pedido = new Pedido(emailCLiente);
        var pagamentoService = new PagamentoService();
        //var comunicacao = new Comunicacao(mediator);



        var escolhendoProdutos = true;

        while (escolhendoProdutos)
        {
            try
            {
                var produtos = await ListarProdutos();
            }
            catch (Exception ex)
            {

                throw;
            }
            var produtos2 = await ListarProdutos();

            var produtoSelecionado = SelecionarProduto(produtos2);
            var quantidade = DefinirQuantidade();

            Console.WriteLine($"\n\nPara sair digite S ou ENTER para continuar escolhendo produtos.");
            var comando = Console.ReadKey();

            if (comando.KeyChar == 's' || comando.KeyChar == 'S')
                escolhendoProdutos = false;

            //pedido = await pedidoRepository.ObterPedido(pedido.IdPedido);

            pedido.AdicionarItem(new ItemPedido(produtoSelecionado, quantidade, pedido.DataPedido));

            Console.Clear();
        }
        await _pedidoRepository.AdicionarPedido(pedido);

        Console.WriteLine("Para seguir com o pagamento digite '1', para cancelar o pedido qualquer tecla.");
        var seguirPagamento = Console.ReadKey().KeyChar - '0' == 1;

        if (!seguirPagamento)
        {
            var pedidoCancelado = pedido.CancelarPedido();
            if (pedidoCancelado)
            {
                Console.WriteLine("Pedido Cancelado");
                await _pedidoRepository.AtualizarPedido(pedido);
                return;
            }
        }

        Console.WriteLine($"Valor total do pedido: {pedido.ValorTotal:C}");
        Console.WriteLine($"Estado do pedido: {pedido.Estado}");

        if (pedido.Itens.Any(i => i.DescontoAplicado))
        {
            Console.WriteLine("Descontos:");
            foreach (var item in pedido.Itens)
            {
                if (item.DescontoAplicado)
                    Console.WriteLine($"{item.Produto.Desconto.GetType().Name} aplicado para o item {item.Produto.Descricao} no valor de {item.ValorDesconto}");
            }
        }



        //Pagamento
        await ProcessarPagamento(pedido.IdPedido);

        var pagamento = await pagamentoService.ProcessarPagamento(pedido.Pagamento);

        if (pagamento)
        {
            pedido.ConcluindoPagamento();
            pedido.AdicionarEvento(new EnviarSeparacaoEstoqueEvent(pedido.IdPedido));
            //await comunicacao.PublicarDomainEvent(new EnviarSeparacaoEstoqueEvent(pedido.IdPedido));
        }

        await _pedidoRepository.AtualizarPedido(pedido);
    }

    public async Task ProcessarPagamento(Guid pedidoId)
    {
        var pedido = await _pedidoRepository.ObterPedido(pedidoId);
        pedido.ProcessandoPagamento();
        await _pedidoRepository.AtualizarPedido(pedido);
    }

    static int DefinirQuantidade()
    {
        Console.WriteLine("Digite a quantidade: ");
        var inputQuantidade = Console.ReadLine();

        if (int.TryParse(inputQuantidade, out int quantidade))
            return quantidade;

        return 0;
    }

    public async Task<List<Produto>> ListarProdutos()
    {
        var produtos = await _produtoRepository.ObterProdutos();

        Console.WriteLine("Lista de produtos");

        foreach (var produto in produtos)
            Console.WriteLine($"Código do produto: {produto.Codigo} - Descricão: {produto.Descricao} - Valor: {produto.PrecoUnitario}");

        return produtos;
    }

    static Produto SelecionarProduto(List<Produto> produtos)
    {
        Console.WriteLine("Digite o código do produto: ");
        var codigo = Console.ReadLine();
        var produtoSelecionado = produtos.Where(p => p.Codigo == codigo).FirstOrDefault();
        return produtoSelecionado;
    }
}