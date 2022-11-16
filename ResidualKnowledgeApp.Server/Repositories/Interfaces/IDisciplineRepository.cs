using ResidualKnowledgeApp.Server.Repositories.Common;

namespace ResidualKnowledgeApp.Server.Repositories
{
	public interface IDisciplineRepository : ICrudRepository<Discipline>
	{
		Task<Discipline> GetWithCompetencesAsync(int disciplineId);

		Task AddCompetences(int disciplineId, List<Competence> competences);
	}
}