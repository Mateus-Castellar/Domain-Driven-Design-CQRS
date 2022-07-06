using DDD.Core.Messages.CommonMessages.DomainEvents;

namespace DDD.Catalogo.Domain.Events
{
    public class ProdutoEstoqueAbaixoEvent : DomainEvent
    {
        public int QuantidadeRestanteNoEstoque { get; private set; }

        public ProdutoEstoqueAbaixoEvent(Guid aggregateId, int quantidadeRestanteNoEstoque) : base(aggregateId)
        {
            QuantidadeRestanteNoEstoque = quantidadeRestanteNoEstoque;
        }
    }
}
