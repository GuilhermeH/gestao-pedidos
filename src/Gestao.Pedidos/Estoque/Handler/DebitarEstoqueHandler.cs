using Gestao.Pedidos.Estoque.Eventos;
using Gestao.Pedidos.Recepcao;
using MediatR;

namespace Gestao.Pedidos.Estoque.Handler
{
    public class DebitarEstoqueHandler(PedidoRepository pedidoRepository, ProdutoRepository produtoRepository) : INotificationHandler<DebitarEstoqueEvent>
    {
        public async Task Handle(DebitarEstoqueEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"{nameof(DebitarEstoqueHandler)} - {notification.ItemPedido.Id}");
            var produto = await produtoRepository.ObterProduto(notification.ItemPedido.ProdutoId);
            var sucesso = produto.DebitarEstoque(notification.ItemPedido.Quantidade);

            var pedido = await pedidoRepository.ObterPedido(notification.PedidoId);

            if (sucesso)
            {
                pedido.Concluir();
            }
            else
            {
                pedido.AguardandoEstoque();
                produto.AdicionarEvento(new AvisoEstoqueAbaixoEvent(produto.Descricao, "Produto abaixo do estoque."));
            }

            await pedidoRepository.AtualizarPedido(pedido);
        }
    }
}
