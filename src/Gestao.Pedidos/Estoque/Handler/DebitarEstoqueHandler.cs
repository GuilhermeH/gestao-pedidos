using Gestao.Pedidos.Estoque.Eventos;
using Gestao.Pedidos.Recepcao;
using MediatR;

namespace Gestao.Pedidos.Estoque.Handler
{
    public class DebitarEstoqueHandler(PedidoRepository pedidoRepository, ProdutoRepository produtoRepository) : INotificationHandler<DebitarEstoqueEvent>
    {
        public async Task Handle(DebitarEstoqueEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine(nameof(DebitarEstoqueHandler));
            var produto = await produtoRepository.ObterProduto(notification.ItemPedido.ProdutoId);
            var sucesso = produto.DebitarEstoque(notification.ItemPedido.Quantidade);

            var pedido = await pedidoRepository.ObterPedido(notification.PedidoId);

            if (!sucesso)
            {
                pedido.AguardandoEstoque();
                produto.AdicionarEvento(new AvisoEstoqueAbaixoEvent(produto.Descricao, $"Produto abaixo do estoque."));
                return;
            }

            pedido.Concluir();
            await pedidoRepository.AtualizarPedido(pedido);
        }
    }
}
