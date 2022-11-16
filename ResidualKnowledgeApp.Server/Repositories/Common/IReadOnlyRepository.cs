using System.Linq.Expressions;

namespace ResidualKnowledgeApp.Server.Repositories.Common
{
	public interface IReadOnlyRepository<TEntity> where TEntity : IEntity
	{
		IQueryable<TEntity> GetAll();

		IQueryable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate);

		Task<TEntity> GetAsync(int id);

		Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);
	}
}
