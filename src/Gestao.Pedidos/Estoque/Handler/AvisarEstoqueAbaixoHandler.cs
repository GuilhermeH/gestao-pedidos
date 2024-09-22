using Gestao.Pedidos.Recepcao.Eventos;
using MediatR;

namespace Gestao.Pedidos.Estoque.Handler;

public class AvisarEstoqueAbaixoHandler : INotificationHandler<AvisoEstoqueAbaixoEvent>
{
    public Task Handle(AvisoEstoqueAbaixoEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Enviando e-mail para vendas: O produto {notification.Produto} está abaixo do estoque.");
        throw new NotImplementedException();
    }
}
