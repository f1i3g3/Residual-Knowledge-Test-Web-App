using Microsoft.EntityFrameworkCore;
using ResidualKnowledgeTestApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ResidualKnowledgeTestApp.Server.Repositories.Common
{
    public class ReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity>
        where TEntity : class, IEntity
    {
        protected readonly DbContext Context;

        public ReadOnlyRepository(DbContext context)
        {
            Context = context;
        }

        public IQueryable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().AsNoTracking();
        }

        public IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().AsNoTracking().Where(predicate);
        }

        public async Task<TEntity> GetAsync(int id)
        {
            return await Context.FindAsync<TEntity>(id);
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(predicate);
        }
    }
}
