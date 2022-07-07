using DDD.Core.Communication.Mediator;
using DDD.Core.DomainObjects;

namespace DDD.Vendas.Data
{
    public static class MediatorExtension
    {
        public static async Task PublicarEventos(this IMediatorHandler mediator, VendasContext context)
        {
            var domainEntities = context.ChangeTracker
                .Entries<Entity>()
                .Where(lbda => lbda.Entity.Notificacoes != null && lbda.Entity.Notificacoes.Any());

            var domainEvents = domainEntities
                .SelectMany(lbda => lbda.Entity.Notificacoes)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.LimparEventos());

            var tasks = domainEvents
                .Select(async (domainEvents) =>
                {
                    //publica um a um (evento..)
                    await mediator.PublicarEvento(domainEvents);
                });
            await Task.WhenAll(tasks);
        }
    }
}