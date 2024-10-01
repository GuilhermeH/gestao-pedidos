using Gestao.Pedidos.Pagamentos;
using Gestao.Pedidos.Recepcao.Eventos;
using Gestao.Pedidos.Shared;

namespace Gestao.Pedidos.Recepcao
{
    public class Pedido : Entity
    {
        protected Pedido()
        {
            
        }
        public Guid IdPedido { get; private set; }
        public List<ItemPedido> Itens { get; } = [];
        public EstadoPedido Estado { get; private set; }
        public DateTime DataPedido { get; private set; }
        public decimal ValorTotal { get; private set; }
        public Pagamento Pagamento { get; private set; }
        public string EmailCliente { get; private set; }

        public Pedido(string emailCliente)
        {
            EmailCliente = emailCliente;
            IdPedido = Guid.NewGuid();
            Estado = EstadoPedido.AguardandoProcessamento;
            DataPedido = DateTime.Now;
        }

        public bool EstoqueEmFalta()
        {
            return Itens.Any(c => c.Produto.QuantidadeEstoque < c.Quantidade);
        }

        public void AdicionarItem(ItemPedido item)
        {
            Itens.Add(item);
            CalcularValorTotal();
        }

        public void CalcularValorTotal()
        {
            decimal total = 0;

            foreach (var item in Itens)
            {
                total += item.Subtotal;
            }

            ValorTotal = total;
        }

        public bool CancelarPedido()
        {
            if (Estado == EstadoPedido.AguardandoProcessamento)
            {
                Estado = EstadoPedido.Cancelado;
                AdicionarEvento(new AvisarClienteAlteracaoEstadoPedidoEvent(IdPedido, Estado, EmailCliente));
                return true;
            }
            return false;
        }

        public void Concluir()
        {
            Estado = EstadoPedido.Concluido;
        }

        public void ProcessandoPagamento()
        {
            Estado = EstadoPedido.ProcessandoPagamento;
            AdicionarEvento(new AvisarClienteAlteracaoEstadoPedidoEvent(IdPedido, Estado, EmailCliente));
        }

        public void AguardandoEstoque()
        {
            Estado = EstadoPedido.AguardandoEstoque;
            AdicionarEvento(new AvisarClienteAlteracaoEstadoPedidoEvent(IdPedido, Estado, EmailCliente));
        }

        public void ConcluindoPagamento()
        {
            Estado = EstadoPedido.PagamentoConcluido;
            AdicionarEvento(new AvisarClienteAlteracaoEstadoPedidoEvent(IdPedido, Estado, EmailCliente));
        }

        public void SeparandoPedido()
        {
            Estado = EstadoPedido.SeparandoPedido;
            AdicionarEvento(new AvisarClienteAlteracaoEstadoPedidoEvent(IdPedido, Estado, EmailCliente));
        }
    }
}
