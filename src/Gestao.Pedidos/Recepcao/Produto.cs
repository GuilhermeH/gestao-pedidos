using Gestao.Pedidos.Recepcao.Eventos;
using Gestao.Pedidos.Shared;

namespace Gestao.Pedidos.Recepcao;

public class Produto : Entity
{
    protected Produto() //EF
    {
        
    }
    public Produto(string descricao, decimal precoUnitario, int quantidadeEstoque, IDesconto desconto)
    {
        Id = Guid.NewGuid();
        Codigo = Guid.NewGuid().ToString();
        Descricao = descricao;
        PrecoUnitario = precoUnitario;
        QuantidadeEstoque = quantidadeEstoque;
        Desconto = desconto;
    }

    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Descricao { get; }
    public decimal PrecoUnitario { get; }
    public int QuantidadeEstoque { get; private set; }
    public IDesconto Desconto { get; }
    public bool DebitarEstoque(int quantidadeVendida)
    {
        if (quantidadeVendida < QuantidadeEstoque)
        {
            AdicionarEvento(new AvisoEstoqueAbaixoEvent(Descricao, "Produto abaixo do estoque."));
            return false;
        }

        QuantidadeEstoque = -quantidadeVendida;
        return true;
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


