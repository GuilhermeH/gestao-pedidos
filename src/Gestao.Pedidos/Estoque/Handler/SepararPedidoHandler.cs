using Gestao.Pedidos.Estoque.Eventos;
using Gestao.Pedidos.Pagamentos.Eventos;
using Gestao.Pedidos.Recepcao;
using MediatR;

namespace Gestao.Pedidos.Estoque.Handler
{
    public class SepararPedidoHandler(PedidoRepository pedidoRepository) : INotificationHandler<EnviarSeparacaoEstoqueEvent>
    {
        public async Task Handle(EnviarSeparacaoEstoqueEvent notification,
            CancellationToken cancellationToken)
        {
            Console.WriteLine(nameof(SepararPedidoHandler));
            var pedido = await pedidoRepository.ObterPedido(notification.PedidoId);

            if (pedido.Estado == EstadoPedido.PagamentoConcluido)
                pedido.SeparandoPedido();

            pedido.Itens.ForEach(item => item.AdicionarEvento(new DebitarEstoqueEvent(item, pedido.IdPedido)));

            //if (pedido.EstoqueEmFalta())
            //{
            //    pedido.AguardandoEstoque();
            //    await pedidoRepository.Commit();
            //    return;
            //}

            //pedido.Itens.ForEach(i => estoqueService.DebitarEstoque(i.Produto.Codigo, i.Quantidade));

            await pedidoRepository.AtualizarPedido(pedido);
        }
    }
}

