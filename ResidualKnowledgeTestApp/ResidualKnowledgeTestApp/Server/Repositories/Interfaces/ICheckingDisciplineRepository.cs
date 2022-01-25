using ResidualKnowledgeTestApp.Server.Repositories.Common;
using ResidualKnowledgeTestApp.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ResidualKnowledgeTestApp.Server.Repositories
{
    public interface ICheckingDisciplineRepository : ICrudRepository<CheckingDiscipline>
    {
        Task UpdateCheckingCompetences(int checkingDisciplineId, List<Competence> competences);
        Task<List<CheckingDiscipline>> GetProjectCheckingDisciplinesAsync(int projectId);
        Task<List<CheckingDiscipline>> GetProjectCheckingDisciplinesWithoutNavigationPropertiesAsync(int projectId);
        Task SetMarkCriteriaCompetences(int checkingDisciplineId, List<MarkCriterion> markCriteria);
        Task<List<MarkCriterion>> GetMarkCriteria(int checkingDisciplineId);
    }
}