namespace Gestao.Pedidos.Recepcao
{
    public class Pedido
    {
        public List<ItemPedido> Itens { get; } = [];
        public EstadoPedido Estado { get; private set; }
        public DateTime DataPedido { get; }
        public decimal ValorTotal { get; private set; }

        public Pedido()
        {
            Estado = EstadoPedido.AguardandoProcessamento;
            DataPedido = DateTime.Now;
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
    }
}
