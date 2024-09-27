using Gestao.Pedidos.Shared;

namespace Gestao.Pedidos.Recepcao
{
    public class ItemPedido : Entity
    {
        protected ItemPedido()
        {
            Id = Guid.NewGuid();
        }
        public Guid Id { get; private set; }
        public Produto Produto { get; private set; }
        public Guid ProdutoId { get; private set; }
        public int Quantidade { get; private set; }
        public decimal ValorUnitario { get; private set; }
        public decimal ValorDesconto { get; private set; }
        public decimal Subtotal => (Produto.PrecoUnitario * Quantidade) - ValorDesconto;
        
        public ItemPedido(Produto produto, int quantidade, DateTime dataPedido)
        {
            Produto = produto;
            Quantidade = quantidade;
            
            ValorUnitario = produto.PrecoUnitario;

            ValorDesconto = Produto.Desconto?.CalcularValor(Produto.PrecoUnitario, Quantidade, dataPedido) ?? 0; ;
        }

        public bool DescontoAplicado => ValorDesconto > 0;
    }
}
