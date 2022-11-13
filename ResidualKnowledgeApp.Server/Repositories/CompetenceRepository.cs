using Microsoft.EntityFrameworkCore;
using ResidualKnowledgeApp.Server.Repositories.Common;
using System.Linq.Expressions;

namespace ResidualKnowledgeApp.Server.Repositories
{
    public class CompetenceRepository : CrudRepository<Competence>, ICompetenceRepository
    {
        public CompetenceRepository(ProjectContext context)
            : base(context)
        {

        }

        public async Task<List<Competence>> GetFilteredCompetences(Expression<Func<Competence, bool>> p)
        {
            var res = await Context.Set<Competence>()
                .AsNoTracking()
                .Where(p)
                .ToListAsync();

            return res;
        }

    }
}