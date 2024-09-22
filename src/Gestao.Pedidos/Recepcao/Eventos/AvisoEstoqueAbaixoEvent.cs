using Gestao.Pedidos.Shared;

namespace Gestao.Pedidos.Recepcao.Eventos
{
    public class AvisoEstoqueAbaixoEvent(string produto, string mensagem) : DomainEvent
    {
        public string Produto { get; } = produto;
        public string Mensagem { get; } = mensagem;
    }
}
