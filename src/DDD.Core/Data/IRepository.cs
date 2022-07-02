using DDD.Core.DomainObjects;

namespace DDD.Core.Data
{
    //Um repo por raiz de agregação
    public interface IRepository<T> : IDisposable where T : IAggregateRoot
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
