using DDD.Core.DomainObjects;
using DDD.Pagamentos.Business.Entities.Enums;

namespace DDD.Pagamentos.Business.Entities
{
    public class Transacao : Entity
    {
        public Guid PedidoId { get; set; }
        public Guid PagamentoId { get; set; }
        public decimal Total { get; set; }
        public StatusTransacao StatusTransacao { get; set; }

        //EfCore Relation
        public Pagamento Pagamento { get; set; }
    }
}