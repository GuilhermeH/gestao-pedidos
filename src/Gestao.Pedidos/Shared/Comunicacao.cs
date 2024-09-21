using MediatR;

namespace Gestao.Pedidos.Shared
{
    public class Comunicacao
    {
        private readonly IMediator _mediator;

        public Comunicacao(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task PublicarDomainEvent<T>(T notificacao) where T : DomainEvent
        {
             await _mediator.Publish(notificacao);
        }
    }
}
