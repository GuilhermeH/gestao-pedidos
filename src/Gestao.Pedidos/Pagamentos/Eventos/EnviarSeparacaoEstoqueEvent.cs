using Gestao.Pedidos.Recepcao;
using Gestao.Pedidos.Shared;

namespace Gestao.Pedidos.Pagamentos.Eventos
{
    public class EnviarSeparacaoEstoqueEvent(Guid pedidoId) : DomainEvent
    {
        public Guid PedidoId { get; } = pedidoId;
    }
}
