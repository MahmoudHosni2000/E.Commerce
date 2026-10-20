using E_Commerce.Domain.Entities;
using System.Linq.Expressions;

namespace E_Commerce.Domain.Contracts
{
    public interface IRepository<TEntity, Tkey> where TEntity : Entity<Tkey>
    {
        void Add(TEntity entity);

        void Update(TEntity entity);

        void Remove(TEntity entity);

        Task<TEntity?> GetByIdAsync(Tkey id);

        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<IEnumerable<TEntity>> GetAllAsync(params Expression<Func<TEntity, object>>[] includes);
        Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity> specification);
        Task<int> CountAsync(ISpecification<TEntity> specification);
    }
}
