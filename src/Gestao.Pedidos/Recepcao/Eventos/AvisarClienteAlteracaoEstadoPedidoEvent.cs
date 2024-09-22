using Gestao.Pedidos.Shared;

namespace Gestao.Pedidos.Recepcao.Eventos
{
    public class AvisarClienteAlteracaoEstadoPedidoEvent(EstadoPedido estadoPedido, string emailCliente) : DomainEvent
    {
        public EstadoPedido EstadoPedido { get; } = estadoPedido;
        public string EmailCliente { get; } = emailCliente;
    }
}
