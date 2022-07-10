using DDD.Core.DomainObjects;

namespace DDD.Pagamentos.Business.Entities
{
    public class Pagamento : Entity, IAggregateRoot
    {
        public Guid PedidoId { get; set; }
        public string Status { get; set; }
        public decimal Valor { get; set; }

        public string NomeCartao { get; set; }
        public string NumeroCartao { get; set; }
        public string ExpiracaoCartao { get; set; }
        public string CvvCartao { get; set; }

        //EfCore Relation
        public Transacao Transacao { get; set; }
    }
}
