using ResidualKnowledgeTestApp.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ResidualKnowledgeTestApp.Server.Services
{
    public interface IProjectsService
    {
        Task<IEnumerable<Project>> GetAllProjectsAsync();

        Task<bool> DoesProjectExist(int projectID);
        
        Task<Project> GetProjectAsync(int projectId);

        Task DeleteProjectAsync(int projectId);

        Task<Project> CreateProjectAsync(Project projectVM); // createProjectViewModel better

        Task UpdateProjectAsync(int projectId, object update);

        Task<List<CheckingDiscipline>> GetProjectCheckingDisciplinesAsync(int id);
        Task<List<DisciplineCompetence>> GetProjectCompetencesForSelection(int id);
    }
}