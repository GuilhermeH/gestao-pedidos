using Gestao.Pedidos.Recepcao.Eventos;
using MediatR;

namespace Gestao.Pedidos.Recepcao.Handlers
{
    public class EnviarEmailAlteracaoEstadoPedidoHandler : INotificationHandler<AvisarClienteAlteracaoEstadoPedidoEvent>
    {
        public Task Handle(AvisarClienteAlteracaoEstadoPedidoEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Enviando email para {notification.EmailCliente}: Seu pedido está em {notification.EstadoPedido}");
            throw new NotImplementedException();
        }
    }
}
