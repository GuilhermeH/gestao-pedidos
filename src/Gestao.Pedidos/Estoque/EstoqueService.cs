using Gestao.Pedidos.Recepcao;

namespace Gestao.Pedidos.Estoque
{
    public class EstoqueService
    {
        private readonly PedidoRepository _pedidoRepository;

        public EstoqueService()
        {
            _pedidoRepository = new PedidoRepository();
        }

        public async Task<bool> DebitarEstoque(string codigo, int quantidadeVendida)
        {
            var produto = await _pedidoRepository.ObterProduto(codigo);
            produto.DebitarEstoque(quantidadeVendida);
            return await _pedidoRepository.Commit();
        }
    }
}
