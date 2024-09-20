
using Gestao.Pedidos.Recepcao;

var pedido = new Pedido();

var produtoA = new Produto("Produto A", 100m, new DescontoSazonal(DateTime.Now.AddDays(-15), DateTime.Now.AddDays(10), 3));
var produtoB = new Produto("Produto B", 100m, new DescontoQuantidade(10, 15));

var item1 = new ItemPedido(produtoA, 5, pedido.DataPedido);
var item2 = new ItemPedido(produtoB, 10, pedido.DataPedido);

pedido.AdicionarItem(item1);
pedido.AdicionarItem(item2);

Console.WriteLine($"Valor total do pedido: {pedido.ValorTotal:C}");
Console.WriteLine($"Estado do pedido: {pedido.Estado}");



if (pedido.Itens.Any(i=>i.DescontoAplicado))
{
    Console.WriteLine("Descontos:");
    foreach (var item in pedido.Itens)
	{
		if (item.DescontoAplicado)
			Console.WriteLine($"{item.Produto.Desconto.GetType().Name} aplicado para o item {item.Produto.Descricao} no valor de {item.ValorDesconto}");
	} 
}
else
{
	Console.WriteLine("Nenhum item elegível para desconto");
}

