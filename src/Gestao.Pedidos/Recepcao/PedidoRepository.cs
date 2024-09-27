using Gestao.Pedidos.Context;
using Microsoft.EntityFrameworkCore;

namespace Gestao.Pedidos.Recepcao
{
    public class PedidoRepository(GestaoPedidosDbContext context)
    {
        public async Task<List<Pedido>> ObterPedidosDiaAterior()
        {
            var pedidos = context.Pedidos.Where(c => c.DataPedido.Date == DateTime.Now.AddDays(-1).Date).ToList();
            return pedidos;
        }

        public async Task<Pedido> ObterPedido(Guid pedidoId)
        {
            var pedido = context.Pedidos.Include(p => p.Itens).Where(p => p.IdPedido == pedidoId).FirstOrDefault();
            return pedido;
        }

        public async Task<bool> AtualizarPedido(Pedido pedido)
        {
            context.Pedidos.Attach(pedido);
            return await context.SaveChangesAsync();
        }

        public async Task<bool> AdicionarPedido(Pedido pedido)
        {
            context.Pedidos.Add(pedido);
            return await context.SaveChangesAsync();
        }


    }
}
