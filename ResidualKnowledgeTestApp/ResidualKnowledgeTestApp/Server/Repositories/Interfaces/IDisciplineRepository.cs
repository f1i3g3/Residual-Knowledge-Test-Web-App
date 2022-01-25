using ResidualKnowledgeTestApp.Server.Repositories.Common;
using ResidualKnowledgeTestApp.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ResidualKnowledgeTestApp.Server.Repositories
{
    public interface IDisciplineRepository : ICrudRepository<Discipline>
    {
        Task<Discipline> GetWithCompetencesAsync(int disciplineId);

        Task AddCompetences(int disciplineId, List<Competence> competences);
    }
}