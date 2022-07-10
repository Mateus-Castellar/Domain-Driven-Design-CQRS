using DDD.Core.Communication.Mediator;
using DDD.Core.DomainObjects;

namespace DDD.Pagamentos.Data
{
    public static class MediatorExtension
    {
        public static async Task PublicarEventos(this IMediatorHandler mediator, PagamentosContext ctx)
        {
            var domainEntities = ctx.ChangeTracker
                .Entries<Entity>()
                .Where(lbda => lbda.Entity.Notificacoes != null && lbda.Entity.Notificacoes.Any());

            var domainEvents = domainEntities
                .SelectMany(lbda => lbda.Entity.Notificacoes)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.LimparEventos());

            var tasks = domainEvents
                .Select(async (domainEvent) =>
                {
                    await mediator.PublicarEvento(domainEvent);
                });

            await Task.WhenAll(tasks);
        }
    }
}