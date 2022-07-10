using DDD.Core.Messages;

namespace DDD.Vendas.Application.Commands
{
    public class CancelarProcessamentoPedidoEstonarEstoqueCommand : Command
    {
        public Guid PedidoId { get; set; }
        public Guid ClienteId { get; set; }

        public CancelarProcessamentoPedidoEstonarEstoqueCommand(Guid pedidoId,
                                                                Guid clienteId)
        {
            AggregateId = pedidoId;
            PedidoId = pedidoId;
            ClienteId = clienteId;
        }
    }
}
