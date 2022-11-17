namespace ResidualKnowledgeApp.Server.Services
{
	public interface ICheckingDisciplinesService
	{
		Task<int> CreateCheckingDisciplineAsync(CheckingDiscipline disciplineVM);

		Task DeleteCheckingDisciplineAsync(int checkingDisciplineId);

		Task<bool> DoesCheckingDisciplineExist(int checkingDisciplineId);

		Task<IEnumerable<CheckingDiscipline>> GetAllCheckingDisciplinesAsync();
		
		Task<CheckingDiscipline> GetCheckingDisciplineAsync(int checkingDisciplineId);
		
		Task UpdateCheckingDisciplineAsync(int checkingDisciplineId, CheckingDiscipline update);

		Task UpdateCheckingDisciplineCheckingCompetencesAsync(int checkingDisciplineId, List<Competence> competences);
		Task<List<CheckingDiscipline>> GetProjectCheckingDisciplinesAsync(int projectId);
		Task<List<CheckingDiscipline>> GetPlainProjectCheckingDisciplinesAsync(int projectId);
		Task SetMarkCriteria(int checkingDisciplineId, List<MarkCriterion> markCriteria);
		Task<List<MarkCriterion>> GetMarkCriteria(int checkingDisciplineId);
	}
}