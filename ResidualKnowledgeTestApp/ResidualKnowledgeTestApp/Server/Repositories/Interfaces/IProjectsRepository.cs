using ResidualKnowledgeTestApp.Server.Repositories.Common;
using ResidualKnowledgeTestApp.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ResidualKnowledgeTestApp.Server.Repositories
{
    public interface IProjectsRepository : ICrudRepository<Project>
    {
        Task<Project> GetWithCheckingDisciplinesWithDisciplineAsync(int projectId);
        
        Task<Project> GetWithEverythingAsync(int projectId);
        
        Task<List<Project>> GetAllWithCurriculumAsync();

        Task<string> GetSheetLink(int projectId);

        Task UpdateSheetLink(int projectId, string link);
    }
}