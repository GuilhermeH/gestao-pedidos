namespace Gestao.Pedidos.Pagamento
{
    public interface IPagamento { }
    
    public record PagamentoPix(decimal PorcetagemDesconto) : IPagamento;

    public record PagamentoParcelado(): IPagamento;
}
