using System.Linq.Expressions;

namespace ResidualKnowledgeApp.Server.Services
{
	public interface ICurriculumsService
	{
		Task<List<Curriculum>> GetAllCurriculumsWithDisciplinesAsync();
		
		Task<bool> DoesCurriculumExist(int curriculumId);
		
		Task<Curriculum> GetCurriculumWithDisciplinesAsync(int curriculumId);
		
		Task<int> CreateCurriculumAsync(Curriculum curriculum);
		
		Task DeleteCurriculumAsync(int curriculumId);

		Task UpdateCurriculumAsync(int curriculumId, Curriculum update);

		Task<List<Competence>> GetAllCurriculumCompetencesAsync(int curriculumId);

		Task<List<Discipline>> GetAllCurriculumDisciplinesAsync(int curriculumId);

		Task<List<Curriculum>> GetFilteredCurriculums(Expression<Func<Curriculum, bool>> p);
		void Detach(Curriculum curric);
	}
}