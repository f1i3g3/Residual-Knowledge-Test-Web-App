using Microsoft.EntityFrameworkCore;

namespace ResidualKnowledgeApp.Server.Repositories.Common
{
	public class CrudRepository<TEntity> : ReadOnlyRepository<TEntity>, ICrudRepository<TEntity>
		where TEntity : class, IEntity, new()
	{
		public CrudRepository(DbContext context)
			: base(context)
		{
		}

		public async Task<int> AddAsync(TEntity item)
		{
			await Context.Set<TEntity>().AddAsync(item);
			await Context.SaveChangesAsync();

			Context.ChangeTracker.DetectChanges();
			return item.Id;
		}

		public void DetachRange(IEnumerable<TEntity> entities)
		{
			foreach (var e in entities)
			{
				Context.Entry<TEntity>(e).State = EntityState.Detached;
			}
		}

		public void Detach(TEntity entity)
		{
			Context.Entry(entity).State = EntityState.Detached;
		}

		public async Task DeleteAsync(int id)
		{
			var item = await Context.Set<TEntity>().FirstOrDefaultAsync(entity => entity.Id == id);

			Context.Set<TEntity>().Remove(item);
			await Context.SaveChangesAsync();

			Context.ChangeTracker.DetectChanges();
		}

		public async Task UpdateAsync(int id, object item)
		{
			var entity = Context.Set<TEntity>().Find(id);
			if (entity == null)
			{
				return;
			}
			Context.Entry(entity).CurrentValues.SetValues(item);
			await Context.SaveChangesAsync();

			Context.ChangeTracker.DetectChanges();
		}
	}
}
