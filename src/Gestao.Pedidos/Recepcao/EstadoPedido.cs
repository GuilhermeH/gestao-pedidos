﻿namespace Gestao.Pedidos.Recepcao
{
    public enum EstadoPedido
    {
        AguardandoProcessamento,
        Cancelado,
        ProcessandoPagamento,
        PagamentoConcluido,
        SeparandoPedido,
        AguardandoEstoque,
        Concluido
    }
}
