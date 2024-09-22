using Gestao.Pedidos.Pagamento.Eventos;
using MediatR;

namespace Gestao.Pedidos.Estoque.Handler
{
    public class SepararPedidoHandler : INotificationHandler<EnviarSeparacaoEstoqueEvent>
    {
        private readonly PedidoRepository _pedidoRepository;
        private readonly EstoqueService _estoqueService;

        public SepararPedidoHandler()
        {
            _pedidoRepository = new PedidoRepository();
            _estoqueService = new EstoqueService();
        }

        public async Task Handle(EnviarSeparacaoEstoqueEvent notification,
            CancellationToken cancellationToken)
        {
            var pedido = await _pedidoRepository.ObterPedido(notification.PedidoId);
            pedido.SeparandoPedido();

            if (pedido.EstoqueEmFalta())
            {
                pedido.AguardandoEstoque();
                await _pedidoRepository.Commit();
                return;
            }

            pedido.Itens.ForEach(i => _estoqueService.DebitarEstoque(i.Produto.Codigo, i.Quantidade));

            await _pedidoRepository.Commit();
        }
    }

    //public async Task Handle(EnviarSeparacaoEstoqueEvent notification, CancellationToken cancellationToken)
    //{
    //    //var pedido = await _pedidoRepository.ObterPedido(notification.Pedido);
    //    notification.Pedido.SeparandoPedido();
    //    thrw
    //    //SaveChanges

    //}

    //Task<bool> IRequestHandler<EnviarSeparacaoEstoqueEvent>.Handle(EnviarSeparacaoEstoqueEvent request, CancellationToken cancellationToken)
    //{
    //    throw new NotImplementedException();
    //}
}

