
using Gestao.Pedidos.Estoque;
using Gestao.Pedidos.Estoque.Handler;
using Gestao.Pedidos.Pagamento;
using Gestao.Pedidos.Pagamento.Eventos;
using Gestao.Pedidos.Recepcao;
using Gestao.Pedidos.Recepcao.Handlers;
using Gestao.Pedidos.Shared;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var mediator = ConfigurarMediatR();

        Console.WriteLine("Informe um email válido: \n");
        var emailCLiente = Console.ReadLine();

        var pedido = new Pedido(emailCLiente);
        var pagamentoService = new PagamentoService();
        var comunicacao = new Comunicacao(mediator);

        var produtoA = new Produto("Produto A", 100m, 10, new DescontoSazonal(DateTime.Now.AddDays(-15), DateTime.Now.AddDays(10), 3));
        var produtoB = new Produto("Produto B", 100m, 50, new DescontoQuantidade(10, 15));

        var item1 = new ItemPedido(produtoA, 5, pedido.DataPedido);
        var item2 = new ItemPedido(produtoB, 10, pedido.DataPedido);

        pedido.AdicionarItem(item1);
        pedido.AdicionarItem(item2);

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

        Console.WriteLine("Para seguir com o pagamento digite '1', para cancelar o pedido qualquer tecla.");
        var seguirPagamento = Console.ReadKey().KeyChar - '0' == 1;

        if (!seguirPagamento)
        {
            var pedidoCancelado = pedido.CancelarPedido();
            if (pedidoCancelado)
                Console.WriteLine("Pedido Cancelado");
            else
                Console.WriteLine("Erro ao cancelar pedido.");
        }

        pedido.ProcessandoPagamento();

        var pagamento = await pagamentoService.ProcessarPagamento(pedido.Pagamento);

        if (pagamento)
        {
            pedido.ConcluindoPagamento();
            await comunicacao.PublicarDomainEvent(new EnviarSeparacaoEstoqueEvent(pedido.IdPedido));
        }

        else
            pedido.CancelarPedido();
    }

    static IMediator ConfigurarMediatR()
    {
        var services = new ServiceCollection();

        // Registra os serviços do MediatR
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddMediatR(typeof(SepararPedidoHandler).Assembly);
        services.AddMediatR(typeof(AvisarEstoqueAbaixoHandler).Assembly);
        services.AddMediatR(typeof(EnviarEmailAlteracaoEstadoPedidoHandler).Assembly);
        

        // Constrói o ServiceProvider
        var serviceProvider = services.BuildServiceProvider();

        // Resolve o MediatR (usado para enviar comandos e consultas)
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        services.AddScoped<PedidoRepository>();

        services.AddScoped<INotificationHandler<EnviarSeparacaoEstoqueEvent>, SepararPedidoHandler>();

        return mediator;
    }
}