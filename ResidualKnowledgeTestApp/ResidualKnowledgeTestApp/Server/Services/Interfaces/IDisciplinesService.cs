using ResidualKnowledgeTestApp.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ResidualKnowledgeTestApp.Server.Services
{
    public interface IDisciplinesService
    {
        Task<int> CreateDisciplineAsync(Discipline discipline);
        void Detach(Discipline discipline);
        Task<Discipline> GetAsync(int id);
        Task Update(Discipline disc);

        Task AddCompetences(int disciplineId, List<Competence> competences);
        Task CreateDisciplinesAsync(int curriculumId, IEnumerable<CurriculumParser.DisciplineImplementation> filteredDisciplineImplementations);
    }
}
