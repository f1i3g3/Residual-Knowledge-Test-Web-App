using ResidualKnowledgeApp.Server.Repositories.Common;

namespace ResidualKnowledgeApp.Server.Repositories
{
	public class MarkCriterionRepository : CrudRepository<MarkCriterion>, IMarkCriterionRepository
	{
		public MarkCriterionRepository(ProjectContext context)
			: base(context)
		{

		}
	}
}
