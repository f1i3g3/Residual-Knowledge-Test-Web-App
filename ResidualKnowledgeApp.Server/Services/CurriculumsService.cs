using ResidualKnowledgeApp.Server.Repositories;
using System.Linq.Expressions;

namespace ResidualKnowledgeApp.Server.Services
{
	public class CurriculumsService : ICurriculumsService
	{
		private ICurriculumRepository _curriculumsRepository;
		private readonly ILogger<CurriculumsService> _logger;

		public CurriculumsService(ICurriculumRepository curriculumsRepository, ILogger<CurriculumsService> logger)
		{
			_curriculumsRepository = curriculumsRepository;
			_logger = logger;
		}

		public async Task<int> CreateCurriculumAsync(Curriculum curriculum)
		{
			_logger.LogInformation("Creating curriculum...");
			var id = await _curriculumsRepository.AddAsync(curriculum);

			_logger.LogInformation("Returning...");
			return id;
		}

		public async Task DeleteCurriculumAsync(int curriculumId)
		{
			_logger.LogInformation("Deleting curriculum...");
			await _curriculumsRepository.DeleteAsync(curriculumId);
			_logger.LogInformation("Done!");
		}

		public void Detach(Curriculum curric)
		{
			_logger.LogInformation("Detaching curriculum...");
			_curriculumsRepository.Detach(curric);
			_logger.LogInformation("Done!");
		}

		public async Task<bool> DoesCurriculumExist(int curriculumId)
		{
			var curriculum = await _curriculumsRepository.FindAsync(c => c.Id == curriculumId);
			return curriculum != null;
		}

		public async Task<List<Competence>> GetAllCurriculumCompetencesAsync(int curriculumId)
		{
			_logger.LogInformation("Getting all competences...");
			var curriculum = await _curriculumsRepository.GetWithDisciplinesAsync(curriculumId);

			_logger.LogInformation("Returning...");
			return curriculum.Competences;
		}

		public async Task<List<Discipline>> GetAllCurriculumDisciplinesAsync(int curriculumId)
		{
			_logger.LogInformation("Getting all disciplines...");
			var curriculum = await _curriculumsRepository.GetWithDisciplinesAsync(curriculumId);

			_logger.LogInformation("Returning...");
			return curriculum.Disciplines;
		}

		public async Task<List<Curriculum>> GetAllCurriculumsWithDisciplinesAsync()
		{
			_logger.LogInformation("Getting all curriculums...");
			var curriculums = await _curriculumsRepository.GetAllWithDisciplines();

			_logger.LogInformation("Returning...");
			return curriculums;
		}

		public async Task<Curriculum> GetCurriculumWithDisciplinesAsync(int curriculumId)
		{
			_logger.LogInformation("Getting curriculum...");
			var curriculum = await _curriculumsRepository.GetWithDisciplinesAsync(curriculumId);

			_logger.LogInformation("Returning...");
			return curriculum;
		}

		public async Task<List<Curriculum>> GetFilteredCurriculums(Expression<Func<Curriculum, bool>> p)
		{
			_logger.LogInformation("Getting filtered curriculums...");
			return await _curriculumsRepository.GetFilteredCurriculums(p);
		}

		public async Task UpdateCurriculumAsync(int curriculumId, Curriculum update)
		{
			_logger.LogInformation("Updating curriculum...");
			await _curriculumsRepository.UpdateAsync(curriculumId,
				new Curriculum()
				{
					Disciplines = update.Disciplines,
					Code = update.Code,
					ProgrammeCode = update.ProgrammeCode,
					ProgrammeName = update.ProgrammeName,
					LevelOfEducation = update.LevelOfEducation
				});
		}
	}
}
