using ResidualKnowledgeTestApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ResidualKnowledgeTestApp.Server.Repositories.Common
{
    public interface ICrudRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : IEntity
    {
        Task<int> AddAsync(TEntity item);

        Task DeleteAsync(int id);
        
        Task UpdateAsync(int id, object item);

        void Detach(TEntity entity);

        void DetachRange(IEnumerable<TEntity> entities);
    }
}
