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

            //// Aplicar possíveis descontos
            //total -= AplicarDescontoPorQuantidade();
            //total -= AplicarDescontoSazonal();

            ValorTotal = total;
        }

        private decimal AplicarDescontoPorQuantidade()
        {
            decimal desconto = 0;

            foreach (var item in Itens)
            {
                // Exemplo de regra de desconto por quantidade
                if (item.Quantidade >= 10)
                {
                    desconto += item.Subtotal * 0.1m; // 10% de desconto para 10 ou mais unidades
                }
            }

            return desconto;
        }

        private decimal AplicarDescontoSazonal()
        {
            decimal desconto = 0;

            // Exemplo de regra de desconto sazonal
            if (DataPedido.Month == 12) // Dezembro, por exemplo
            {
                desconto = ValorTotal * 0.15m; // 15% de desconto
            }

            return desconto;
        }

        public void CancelarPedido()
        {
            if (Estado == EstadoPedido.AguardandoProcessamento)
            {
                Estado = EstadoPedido.Cancelado;
            }
            else
            {
                throw new InvalidOperationException("O pedido não pode ser cancelado.");
            }
        }
    }
}
