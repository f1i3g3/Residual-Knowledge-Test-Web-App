using ResidualKnowledgeApp.Server.Repositories.Common;
using System.Linq.Expressions;

namespace ResidualKnowledgeApp.Server.Repositories
{
    public interface ICompetenceRepository : ICrudRepository<Competence>
    {
        Task<List<Competence>> GetFilteredCompetences(Expression<Func<Competence, bool>> p);
    }
}