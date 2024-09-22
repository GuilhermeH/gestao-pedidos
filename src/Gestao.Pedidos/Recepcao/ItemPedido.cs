namespace Gestao.Pedidos.Recepcao
{
    public class ItemPedido
    {
        protected ItemPedido()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; set; }
        public Produto Produto { get; private set; }
        public int Quantidade { get; }
        public decimal ValorDesconto { get; }
        public decimal Subtotal => (Produto.PrecoUnitario * Quantidade) - ValorDesconto;
        
        public ItemPedido(Produto produto, int quantidade, DateTime dataPedido)
        {
            Produto = produto;
            Quantidade = quantidade;

            ValorDesconto = Produto.Desconto?.CalcularValor(Produto.PrecoUnitario, Quantidade, dataPedido) ?? 0; ;
        }

        public bool DescontoAplicado => ValorDesconto > 0;
    }
}
