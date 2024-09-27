using Gestao.Pedidos.Context;
using Microsoft.EntityFrameworkCore;

namespace Gestao.Pedidos.Recepcao
{
    public class ProdutoRepository
    {
        private readonly GestaoPedidosDbContext _gestaoPedidosDbContext;

        public ProdutoRepository(GestaoPedidosDbContext gestaoPedidosDbContext)
        {
            _gestaoPedidosDbContext = gestaoPedidosDbContext;
        }

        public async Task<List<Produto>> ObterProdutos()
        {
            return await _gestaoPedidosDbContext.Produtos.Include(c => c.Desconto).ToListAsync();
        }

        public async Task<Produto> ObterProduto(Guid produtoId)
        {
            var produto = await _gestaoPedidosDbContext.Produtos.FirstOrDefaultAsync(p => p.Id == produtoId);
            return produto;
        }

    } 


    //      public string Codigo { get; set; }
    //public string Descricao { get; }
    //public decimal PrecoUnitario { get; }
    //public int QuantidadeEstoque { get; private set; }
    //public Desconto Desconto { get; private set; }
}
