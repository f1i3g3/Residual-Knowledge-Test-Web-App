namespace ResidualKnowledgeApp.Server.Services
{
    public interface IProjectsService
    {
        Task<IEnumerable<Project>> GetAllProjectsAsync();

        Task<bool> DoesProjectExist(int projectID);
        
        Task<Project> GetProjectAsync(int projectId);

        Task DeleteProjectAsync(int projectId);

        Task<Project> CreateProjectAsync(Project projectVM);

        Task UpdateProjectAsync(int projectId, object update);

        Task<List<CheckingDiscipline>> GetProjectCheckingDisciplinesAsync(int id);
        Task<List<DisciplineCompetence>> GetProjectCompetencesForSelection(int id);

        Task<string> GetSheetLink(int projectId);
    }
}