using Gestao.Pedidos.Recepcao;

namespace Gestao.Pedidos.Estoque
{
    public class PedidoRepository
    {
        private readonly List<Produto> Produtos;
        private readonly List<Pedido> Pedidos;
        public async Task<Pedido> ObterPedido(Guid pedidoId)
        {
            //throw new NotImplementedException();
            return await Task.FromResult(Pedidos.FirstOrDefault());
        }

        public async Task<Produto> ObterProduto(string codigo)
        {
            //throw new NotImplementedException();
            return await Task.FromResult(Produtos.FirstOrDefault());
        }

        public async Task<bool> Commit()
        {
            //throw new NotImplementedException();
            return true;
        }


    }
}
