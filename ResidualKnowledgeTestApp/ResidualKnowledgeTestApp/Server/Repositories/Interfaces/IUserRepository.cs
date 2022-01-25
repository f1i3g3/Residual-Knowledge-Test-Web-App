using ResidualKnowledgeTestApp.Server.Repositories.Common;
using ResidualKnowledgeTestApp.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ResidualKnowledgeTestApp.Server.Repositories
{
    public interface IUserRepository : ICrudRepository<User>
    {
        Task<User> GetProjectUserAsync(int projectId);
    }
}