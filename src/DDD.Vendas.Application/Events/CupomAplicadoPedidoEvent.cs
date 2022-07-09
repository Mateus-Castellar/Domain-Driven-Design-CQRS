using DDD.Core.Messages;

namespace DDD.Vendas.Application.Events
{
    public class CupomAplicadoPedidoEvent : Event
    {
        public Guid ClienteId { get; private set; }
        public Guid PedidoId { get; private set; }
        public Guid CupomId { get; private set; }

        public CupomAplicadoPedidoEvent(Guid clienteId,
                                        Guid pedidoId,
                                        Guid cupomId)
        {
            AggregateId = pedidoId;
            ClienteId = clienteId;
            PedidoId = pedidoId;
            CupomId = cupomId;
        }
    }
}
