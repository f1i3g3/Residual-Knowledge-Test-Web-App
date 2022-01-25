using Microsoft.EntityFrameworkCore;
using ResidualKnowledgeTestApp.Server.Repositories.Common;
using ResidualKnowledgeTestApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ResidualKnowledgeTestApp.Server.Repositories
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