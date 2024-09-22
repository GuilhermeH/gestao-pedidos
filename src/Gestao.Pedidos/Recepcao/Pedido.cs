using Gestao.Pedidos.Pagamento;

namespace Gestao.Pedidos.Recepcao
{
    public class Pedido
    {
        public Guid IdPedido { get; }
        public List<ItemPedido> Itens { get; } = [];
        public EstadoPedido Estado { get; private set; }
        public DateTime DataPedido { get; }
        public decimal ValorTotal { get; private set; }
        public IPagamento Pagamento { get; }

        public Pedido()
        {
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
        }

        public void AguardandoEstoque()
        {
            Estado = EstadoPedido.AguardandoEstoque;
        }

        public void ConcluindoPagamento()
        {
            Estado = EstadoPedido.ProcessandoPagamento;
        }

        public void SeparandoPedido()
        {
            Estado = EstadoPedido.SeprandoPedido;
        }
    }
}
