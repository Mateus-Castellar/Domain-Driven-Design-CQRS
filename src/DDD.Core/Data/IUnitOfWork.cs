namespace DDD.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
