using E_Commerce.Domain.Contracts;
using E_Commerce.Domain.Entities;
using E_Commerce.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Persistence.Repositories
{
    public class UnitOfWork(StoreDbContext context) : IUnitOfWork
    {
        // Dictionary to hold the repositories for different entity types
        private readonly Dictionary<string, object> _repositories = [];

        public IRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : Entity<TKey>
        {
            var typeName = typeof(TEntity).Name;

            if (_repositories.TryGetValue(typeName, out object? value))
                return (value as IRepository<TEntity, TKey>)!;

            var repository = new Repository<TEntity, TKey>(context);

            _repositories.Add(typeName, repository);

            return repository;
        }

        public async Task<int> SaveChangesAsync()
            => await context.SaveChangesAsync();
    }
}