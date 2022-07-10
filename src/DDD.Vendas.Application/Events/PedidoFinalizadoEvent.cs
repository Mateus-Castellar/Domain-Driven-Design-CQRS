using DDD.Core.Messages;

namespace DDD.Vendas.Application.Events
{
    public class PedidoFinalizadoEvent : Event
    {
        public Guid PedidoId { get; set; }

        public PedidoFinalizadoEvent(Guid pedidoId)
        {
            AggregateId = pedidoId;
            PedidoId = pedidoId;
        }
    }
}