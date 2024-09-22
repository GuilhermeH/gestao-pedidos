namespace Gestao.Pedidos.Shared
{
    public abstract class Entity
    {
        public List<DomainEvent> Events => new();

        public void AdicionarEvento(DomainEvent evento)
        {
            Events.Add(evento);
        }
    }
}
