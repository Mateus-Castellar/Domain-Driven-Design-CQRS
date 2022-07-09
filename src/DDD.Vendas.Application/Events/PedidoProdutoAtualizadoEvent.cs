using DDD.Core.Messages;

namespace DDD.Vendas.Application.Events
{
    public class PedidoProdutoAtualizadoEvent : Event
    {
        public Guid ClienteId { get; private set; }
        public Guid PedidoId { get; private set; }
        public Guid ProdutoId { get; private set; }
        public int Quantidade { get; set; }

        public PedidoProdutoAtualizadoEvent(Guid clienteId,
                                            Guid pedidoId,
                                            Guid produtoId,
                                            int quantidade)
        {
            AggregateId = pedidoId;
            ClienteId = clienteId;
            PedidoId = pedidoId;
            ProdutoId = produtoId;
            Quantidade = quantidade;
        }
    }
}