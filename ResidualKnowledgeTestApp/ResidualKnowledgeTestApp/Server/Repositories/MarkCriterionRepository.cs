using ResidualKnowledgeTestApp.Server.Repositories.Common;
using ResidualKnowledgeTestApp.Shared;

namespace ResidualKnowledgeTestApp.Server.Repositories
{
    public class MarkCriterionRepository : CrudRepository<MarkCriterion>, IMarkCriterionRepository
    {
        public MarkCriterionRepository(ProjectContext context)
            : base(context)
        {

        }
    }
}
