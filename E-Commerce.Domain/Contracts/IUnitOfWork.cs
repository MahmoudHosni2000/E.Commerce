using E_Commerce.Domain.Entities;

namespace E_Commerce.Domain.Contracts
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();

        IRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : Entity<TKey>;
    }
}