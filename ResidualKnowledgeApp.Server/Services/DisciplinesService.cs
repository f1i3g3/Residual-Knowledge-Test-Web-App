using ResidualKnowledgeApp.Server.Repositories;

namespace ResidualKnowledgeApp.Server.Services
{
	public class DisciplinesService : IDisciplinesService
	{
		private IDisciplineRepository _disciplineRepository;
		private ICompetenceService _competenceService;

		public DisciplinesService(IDisciplineRepository disciplineRepository, ICompetenceService competenceService)
		{
			_disciplineRepository = disciplineRepository;
			_competenceService = competenceService;
		}

		public async Task<int> CreateDisciplineAsync(Discipline discipline)
		{
			var id = await _disciplineRepository.AddAsync(discipline);
			return id;
		}

		public void Detach(Discipline discipline)
		{
			_disciplineRepository.Detach(discipline);
		}

		public async Task<Discipline> GetAsync(int id)
		{
			return await _disciplineRepository.GetWithCompetencesAsync(id);
		}

		public async Task Update(Discipline disc)
		{
			await _disciplineRepository.UpdateAsync(disc.Id, disc);
		}

		public async Task AddCompetences(int disciplineId, List<Competence> competences)
		{
			await _disciplineRepository.AddCompetences(disciplineId, competences);
		}

		public async Task CreateDisciplinesAsync(int curriculumId,
			IEnumerable<CurriculumParser.DisciplineImplementation> filteredDisciplineImplementations)
		{
			foreach (var i in filteredDisciplineImplementations)
			{
				var disciplineCompetenceCodes = i.Discipline.Implementations.Where(i => i.Semester <= i.Semester)
					.SelectMany(i => i.Competences).Select(c => c.Code).Distinct().ToList();
				var competences = await _competenceService.GetFiltered(c => c.CurriculumId == curriculumId && disciplineCompetenceCodes.Contains(c.Code));
				var discipline = new Discipline
				{
					Code = i.Discipline.Code,
					Name = i.Discipline.RussianName,
					Semester = i.Semester,
					CurriculumId = curriculumId,
				};
				var id = await CreateDisciplineAsync(discipline);
				Detach(discipline);
				await AddCompetences(id, competences);
				_competenceService.DetachRange(competences);
			}
		}
	}
}
