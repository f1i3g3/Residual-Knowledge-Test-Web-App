using Microsoft.EntityFrameworkCore;
using ResidualKnowledgeApp.Server.Repositories.Common;

namespace ResidualKnowledgeApp.Server.Repositories
{
    public class UserRepository : CrudRepository<User>, IUserRepository
    {
        public UserRepository(ProjectContext context)
            : base(context)
        {

        }

        public async Task<User> GetProjectUserAsync(int projectId)
        {
            return await Context.Set<User>()
                .FirstOrDefaultAsync(u => u.ProjectId == projectId);
        }
    }
}
