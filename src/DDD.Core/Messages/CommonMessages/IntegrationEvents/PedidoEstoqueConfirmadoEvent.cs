using DDD.Core.DomainObjects.DTO;

namespace DDD.Core.Messages.CommonMessages.IntegrationEvents
{
    public class PedidoEstoqueConfirmadoEvent : IntegrationEvent
    {
        public Guid PedidoId { get; private set; }
        public Guid ClienteId { get; private set; }
        public decimal Total { get; private set; }
        public ListaProdutosPedido ProdutosPedidos { get; private set; }
        public string NomeCartao { get; private set; }
        public string NumeroCartao { get; private set; }
        public string ExpiracaoCartao { get; private set; }
        public string CvvCartao { get; private set; }

        public PedidoEstoqueConfirmadoEvent(Guid pedidoId,
                                            Guid clienteId,
                                            decimal total,
                                            ListaProdutosPedido produtosPedidos,
                                            string nomeCartao,
                                            string numeroCartao,
                                            string expiracaoCartao,
                                            string cvvCartao)
        {
            AggregateId = pedidoId;
            PedidoId = pedidoId;
            ClienteId = clienteId;
            Total = total;
            ProdutosPedidos = produtosPedidos;
            NomeCartao = nomeCartao;
            NumeroCartao = numeroCartao;
            ExpiracaoCartao = expiracaoCartao;
            CvvCartao = cvvCartao;
        }
    }
}
