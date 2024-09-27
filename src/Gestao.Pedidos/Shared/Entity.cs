namespace Gestao.Pedidos.Shared
{
    public abstract class Entity
    {
        public List<DomainEvent> Events { get; private set; } = new();

        public void AdicionarEvento(DomainEvent evento)
        {
            Events.Add(evento);
        }

        public void LimparEventos()
        {
            Events?.Clear();
        }
    }
}
