using Microsoft.EntityFrameworkCore;
using ResidualKnowledgeTestApp.Server.Repositories.Common;
using ResidualKnowledgeTestApp.Shared;
using System.Threading.Tasks;

namespace ResidualKnowledgeTestApp.Server.Repositories
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
