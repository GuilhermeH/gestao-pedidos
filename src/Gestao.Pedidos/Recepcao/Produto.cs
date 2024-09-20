namespace Gestao.Pedidos.Recepcao
{
    public record Produto(string Descricao, decimal PrecoUnitario, IDesconto Desconto);

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
