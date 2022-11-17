using ResidualKnowledgeApp.Server.Repositories.Common;

namespace ResidualKnowledgeApp.Server.Repositories
{
	public interface IUserRepository : ICrudRepository<User>
	{
		// Task<User> GetProjectUserAsync(int projectId);
	}
}