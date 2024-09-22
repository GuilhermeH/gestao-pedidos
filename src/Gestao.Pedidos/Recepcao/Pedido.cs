using Gestao.Pedidos.Pagamento;
using Gestao.Pedidos.Recepcao.Eventos;
using Gestao.Pedidos.Shared;

namespace Gestao.Pedidos.Recepcao
{
    public class Pedido : Entity
    {
        public Guid IdPedido { get; }
        public List<ItemPedido> Itens { get; } = [];
        public EstadoPedido Estado { get; private set; }
        public DateTime DataPedido { get; }
        public decimal ValorTotal { get; private set; }
        public IPagamento Pagamento { get; }
        public string EmailCliente { get; }

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
                return true;
            }
            return false;
        }

        public void ProcessandoPagamento()
        {
            Estado = EstadoPedido.ProcessandoPagamento;
            AdicionarEvento(new AvisarClienteAlteracaoEstadoPedidoEvent(Estado, EmailCliente));
        }

        public void AguardandoEstoque()
        {
            Estado = EstadoPedido.AguardandoEstoque;
            AdicionarEvento(new AvisarClienteAlteracaoEstadoPedidoEvent(Estado, EmailCliente));
        }

        public void ConcluindoPagamento()
        {
            Estado = EstadoPedido.ProcessandoPagamento;
            AdicionarEvento(new AvisarClienteAlteracaoEstadoPedidoEvent(Estado, EmailCliente));
        }

        public void SeparandoPedido()
        {
            Estado = EstadoPedido.SeprandoPedido;
            AdicionarEvento(new AvisarClienteAlteracaoEstadoPedidoEvent(Estado, EmailCliente));
        }
    }
}
