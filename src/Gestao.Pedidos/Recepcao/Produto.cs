using Gestao.Pedidos.Recepcao.Eventos;
using Gestao.Pedidos.Shared;

namespace Gestao.Pedidos.Recepcao;

public class Produto : Entity
{
    protected Produto() //EF
    {

    }
    public Produto(string descricao, decimal precoUnitario, int quantidadeEstoque, Desconto desconto)
    {
        Id = Guid.NewGuid();
        var guidString = Id.ToString();
        Codigo = guidString.Substring(guidString.Length -4);
        Descricao = descricao;
        PrecoUnitario = precoUnitario;
        QuantidadeEstoque = quantidadeEstoque;
        Desconto = desconto;
        DescontoId = desconto.Id;
    }

    public Guid Id { get; set; }
    public string Codigo { get; set; }
    public string Descricao { get; }
    public decimal PrecoUnitario { get; }
    public int QuantidadeEstoque { get; private set; }
    public Desconto Desconto { get; private set; }
    public Guid DescontoId { get; private set; }
    public bool DebitarEstoque(int quantidadeVendida)
    {
        if (quantidadeVendida < QuantidadeEstoque)
        {
            //AdicionarEvento(new AvisoEstoqueAbaixoEvent(Descricao, "Produto abaixo do estoque."));
            return false;
        }

        QuantidadeEstoque =- quantidadeVendida;
        return true;
    }
}

public class DescontoSazonal : Desconto
{
    protected DescontoSazonal()
    {
        
    }
    public DescontoSazonal(DateTime dataInicial, DateTime dataFinal, decimal porcentagem)
    {
        DataInicial = dataInicial;
        DataFinal = dataFinal;
        Porcentagem = porcentagem;
    }

    public Guid Id { get; private set; }
    public DateTime DataInicial { get; private set; }
    public DateTime DataFinal { get; private set; }
    public decimal Porcentagem { get; private set; }
    public override decimal CalcularValor(decimal precoUnitario, int quantidade, DateTime dataPedido)
    {
        if (dataPedido >= DataInicial && dataPedido <= DataFinal)
        {
            decimal valorTotal = precoUnitario * quantidade;
            return valorTotal * (Porcentagem / 100);
        }

        return 0;
    }
}

public class DescontoQuantidade : Desconto
{
    protected DescontoQuantidade()
    {
        
    }
    public DescontoQuantidade(int quantidadeAplicavel, decimal valorEmReais)
    {
        QuantidadeAplicavel = quantidadeAplicavel;
        ValorEmReais = valorEmReais;
    }

    public Guid Id { get; private set; }
    public int QuantidadeAplicavel { get; private set; }
    public decimal ValorEmReais { get; private set; }

    public override decimal CalcularValor(decimal precoUnitario, int quantidade, DateTime dataPedido)
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
    public Guid Id { get; }
    decimal CalcularValor(decimal precoUnitario, int quantidade, DateTime dataPedido);
}

public abstract class Desconto : IDesconto
{
    public Guid Id { get; protected set; } = Guid.NewGuid();

    // Método abstrato para calcular o valor do desconto
    public abstract decimal CalcularValor(decimal precoUnitario, int quantidade, DateTime dataPedido);
}

