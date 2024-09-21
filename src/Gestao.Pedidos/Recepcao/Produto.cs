namespace Gestao.Pedidos.Recepcao
{
    public class Produto
    {
        public Produto(string descricao, decimal precoUnitario, int quantidadeEstoque, IDesconto desconto)
        {
            Codigo = Guid.NewGuid().ToString();
            Descricao = descricao;
            PrecoUnitario = precoUnitario;
            QuantidadeEstoque = quantidadeEstoque;
            Desconto = desconto;
        }

        public string Codigo { get; set; }
        public string Descricao { get; }
        public decimal PrecoUnitario { get; }
        public int QuantidadeEstoque { get; private set; }
        public IDesconto Desconto { get; }
        public void DebitarEstoque(int quantidadeVendida)
        {
            QuantidadeEstoque = -quantidadeVendida;
        }
    }

    public record DescontoSazonal : IDesconto
    {
        public DescontoSazonal(DateTime dataInicial, DateTime dataFinal, decimal porcentagem)
        {
            DataInicial = dataInicial;
            DataFinal = dataFinal;
            Porcentagem = porcentagem;
        }

        public DateTime DataInicial { get; }
        public DateTime DataFinal { get; }
        public decimal Porcentagem { get; }
        public int QuantidadeEstoque { get; }

        public decimal CalcularValor(decimal precoUnitario, int quantidade, DateTime dataPedido)
        {
            if (dataPedido >= DataInicial && dataPedido <= DataFinal)
            {
                decimal valorTotal = precoUnitario * quantidade;
                return valorTotal * (Porcentagem / 100);
            }

            return 0;
        }
    }

    public record DescontoQuantidade : IDesconto
    {
        public DescontoQuantidade(int quantidadeAplicavel, decimal valorEmReais)
        {
            QuantidadeAplicavel = quantidadeAplicavel;
            ValorEmReais = valorEmReais;
        }

        public int QuantidadeAplicavel { get; }
        public decimal ValorEmReais { get; }

        public decimal CalcularValor(decimal precoUnitario, int quantidade, DateTime dataPedido)
        {
            if (quantidade >= QuantidadeAplicavel)
            {
                return ValorEmReais * quantidade;
            }

            return 0;
        }
    }

    public interface IDesconto
    {
        decimal CalcularValor(decimal precoUnitario, int quantidade, DateTime dataPedido);
    }

}
