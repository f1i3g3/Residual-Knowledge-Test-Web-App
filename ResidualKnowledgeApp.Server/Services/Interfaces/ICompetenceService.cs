using System.Linq.Expressions;

namespace ResidualKnowledgeApp.Server.Services
{
	public interface ICompetenceService
	{
		Task<int> CreateCompetenceAsync(Competence competence);

		Task<List<Competence>> GetAllCurriculumCompetencesAsync(int curriculumId);

		Task<List<Competence>> GetFiltered(Expression<Func<Competence, bool>> p);
		
		void Detach(Competence competence);

		void DetachRange(IEnumerable<Competence> competences);

		Task CreateCompetencesAsync(int curriculumId, List<CurriculumParser.Competence> competences);
	}
}
