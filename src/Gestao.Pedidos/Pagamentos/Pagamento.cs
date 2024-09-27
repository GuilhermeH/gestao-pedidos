using Gestao.Pedidos.Recepcao;

namespace Gestao.Pedidos.Pagamentos
{
    public class Pagamento
    {
        public Guid Id { get; private set; }
        public Guid PedidoId { get; private set; }
        public Pedido Pedido { get; private set; }
    }

    public class PagamentoPix : Pagamento
    {
        public decimal PorcentagemDesconto { get; private set; }
    }

    public class PagamentoParcelado : Pagamento
    {
        public int NumeroDeParcelas { get; private set; }
    }
}
