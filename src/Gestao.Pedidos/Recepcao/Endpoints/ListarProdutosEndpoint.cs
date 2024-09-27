using Microsoft.AspNetCore.Mvc;

namespace Gestao.Pedidos.Recepcao.Controllers
{
    public class ListarProdutosEndpoint : ControllerBase
    {
        [HttpGet("produtos")]
        public async Task<IActionResult> ListarProdutos([FromServices]ProdutoRepository produtoRepository, CancellationToken cancellationToken)
        {
            var produtos = await produtoRepository.ObterProdutos();
            return Ok(produtos);
        }
    }
}
