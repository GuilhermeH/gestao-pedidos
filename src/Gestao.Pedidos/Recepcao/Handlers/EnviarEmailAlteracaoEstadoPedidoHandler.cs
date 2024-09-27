using Gestao.Pedidos.Recepcao.Eventos;
using MediatR;

namespace Gestao.Pedidos.Recepcao.Handlers
{
    public class EnviarEmailAlteracaoEstadoPedidoHandler : INotificationHandler<AvisarClienteAlteracaoEstadoPedidoEvent>
    {
        public async Task Handle(AvisarClienteAlteracaoEstadoPedidoEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine(nameof(EnviarEmailAlteracaoEstadoPedidoHandler));
            Console.WriteLine($"Enviando email para {notification.EmailCliente}: Seu pedido está em {notification.EstadoPedido}");
        }
    }
}
