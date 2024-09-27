using Gestao.Pedidos.Estoque.Eventos;
using MediatR;

namespace Gestao.Pedidos.Estoque.Handler;

public class AvisarEstoqueAbaixoHandler : INotificationHandler<AvisoEstoqueAbaixoEvent>
{
    public async Task Handle(AvisoEstoqueAbaixoEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine(nameof(AvisarEstoqueAbaixoHandler));
        Console.WriteLine($"Enviando e-mail para vendas: O produto {notification.Produto} está abaixo do estoque.");
        
    }
}
