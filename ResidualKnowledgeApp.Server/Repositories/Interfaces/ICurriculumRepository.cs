using ResidualKnowledgeApp.Server.Repositories.Common;
using System.Linq.Expressions;

namespace ResidualKnowledgeApp.Server.Repositories
{
    public interface ICurriculumRepository : ICrudRepository<Curriculum>
    {
        Task<Curriculum> GetWithDisciplinesAsync(int curriculumId);

        Task<List<Curriculum>> GetAllWithDisciplines();

        Task<List<Curriculum>> GetFilteredCurriculums(Expression<Func<Curriculum, bool>> p);
    }
}