using ResidualKnowledgeTestApp.Server.Repositories.Common;
using ResidualKnowledgeTestApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ResidualKnowledgeTestApp.Server.Repositories
{
    public interface ICompetenceRepository : ICrudRepository<Competence>
    {
        Task<List<Competence>> GetFilteredCompetences(Expression<Func<Competence, bool>> p);
    }
}