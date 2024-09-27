using Gestao.Pedidos.Recepcao;
using Gestao.Pedidos.Shared;

namespace Gestao.Pedidos.Estoque.Eventos
{
    public class DebitarEstoqueEvent(ItemPedido itemPedido, Guid pedidoId) : DomainEvent
    {
        public ItemPedido ItemPedido = itemPedido;
        public Guid PedidoId = pedidoId;
    }

   
}
