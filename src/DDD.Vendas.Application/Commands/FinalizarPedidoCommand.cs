using DDD.Core.Messages;

namespace DDD.Vendas.Application.Commands
{
    public class FinalizarPedidoCommand : Command
    {
        public Guid PedidoId { get; set; }
        public Guid ClienteId { get; set; }

        public FinalizarPedidoCommand(Guid pedidoId,
                                      Guid clienteId)
        {
            AggregateId = pedidoId;
            PedidoId = pedidoId;
            ClienteId = clienteId;
        }
    }
}
