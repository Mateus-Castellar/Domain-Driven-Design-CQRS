using DDD.Core.DomainObjects.DTO;
using DDD.Core.Messages;

namespace DDD.Vendas.Application.Events
{
    public class PedidoProcessamentoCanceladoEvent : Event
    {
        public Guid PedidoId { get; set; }
        public Guid ClienteId { get; set; }
        public ListaProdutosPedido ProdutosPedido { get; set; }

        public PedidoProcessamentoCanceladoEvent(Guid pedidoId,
                                                 Guid clienteId,
                                                 ListaProdutosPedido produtosPedido)
        {
            AggregateId = pedidoId;
            PedidoId = pedidoId;
            ClienteId = clienteId;
            ProdutosPedido = produtosPedido;
        }
    }
}