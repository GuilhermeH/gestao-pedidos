using Gestao.Pedidos.Shared;

namespace Gestao.Pedidos.Recepcao.Eventos
{
    public class AvisarClienteAlteracaoEstadoPedidoEvent(Guid pedidoId, EstadoPedido estadoPedido, string emailCliente) : DomainEvent
    {
        public Guid IdPedido { get; } = pedidoId;
        public EstadoPedido EstadoPedido { get; } = estadoPedido;
        public string EmailCliente { get; } = emailCliente;
    }
}
