using Gestao.Pedidos.Recepcao;
using Gestao.Pedidos.Shared;

namespace Gestao.Pedidos.Estoque.Eventos
{
    public class DebitarEstoqueEvent(ItemPedido itemPedido) : DomainEvent
    {
        public ItemPedido ItemPedido = itemPedido;
    }

   
}
