using ResidualKnowledgeTestApp.Server.Repositories.Common;
using ResidualKnowledgeTestApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ResidualKnowledgeTestApp.Server.Repositories
{
    public interface ICurriculumRepository : ICrudRepository<Curriculum>
    {
        Task<Curriculum> GetWithDisciplinesAsync(int curriculumId);

        Task<List<Curriculum>> GetAllWithDisciplines();

        Task<List<Curriculum>> GetFilteredCurriculums(Expression<Func<Curriculum, bool>> p);
    }
}