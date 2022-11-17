using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ResidualKnowledgeApp.Server.Repositories;

namespace ResidualKnowledgeApp.Server.Services
{
	public class CheckingDisciplinesService : ICheckingDisciplinesService
	{
		private readonly IMapper _mapper;
		private readonly ICheckingDisciplineRepository _checkingDisciplineRepository;
		private IMarkCriterionRepository _markCriterionRepository;
		private readonly ILogger<CheckingDisciplinesService> _logger;

		public CheckingDisciplinesService(IMapper mapper, ICheckingDisciplineRepository checkingDisciplineRepository,
			IMarkCriterionRepository markCriterionRepository, ILogger<CheckingDisciplinesService> logger)
		{
			_mapper = mapper;
			_checkingDisciplineRepository = checkingDisciplineRepository;
			_markCriterionRepository = markCriterionRepository;
			_logger = logger;
		}

		/// <summary>
		/// Выбор дисциплин
		/// </summary>
		/// <param name="checkingDiscipline"></param>
		/// <returns></returns>
		public async Task<int> CreateCheckingDisciplineAsync(CheckingDiscipline checkingDiscipline) // createProjectVM
		{
			_logger.LogInformation("Creating checking discipline...");
			var criteria = new List<MarkCriterion>
			{
				  { new MarkCriterion { ECTSMark = 'A', FivePointScaleMark = 5} },
				  { new MarkCriterion { ECTSMark = 'B', FivePointScaleMark = 4} },
				  { new MarkCriterion { ECTSMark = 'C', FivePointScaleMark = 4} },
				  { new MarkCriterion { ECTSMark = 'D', FivePointScaleMark = 3} },
				  { new MarkCriterion { ECTSMark = 'E', FivePointScaleMark = 3} },
				  { new MarkCriterion { ECTSMark = 'F', FivePointScaleMark = 2} },
			};
			criteria.ForEach(c => _markCriterionRepository.AddAsync(c));

			var checkingDisciplineId = await _checkingDisciplineRepository.AddAsync(checkingDiscipline);
			await _checkingDisciplineRepository.SetMarkCriteriaCompetences(checkingDisciplineId, criteria);

			_logger.LogInformation("Returning...");
			return checkingDisciplineId;
		}

		/// <summary>
		/// Отмена выбора дисциплин
		/// </summary>
		/// <param name="checkingDisciplineId"></param>
		/// <returns></returns>
		public async Task DeleteCheckingDisciplineAsync(int checkingDisciplineId)
		{
			_logger.LogInformation("Deleting checking discipline...");
			await _checkingDisciplineRepository.DeleteAsync(checkingDisciplineId);
			_logger.LogInformation("Deleted!");
		}

		/// <summary>
		/// Проверка существования дисциплины
		/// </summary>
		/// <param name="checkingDisciplineId"></param>
		/// <returns></returns>
		public async Task<bool> DoesCheckingDisciplineExist(int checkingDisciplineId)
		{
			var checkingDiscipline = await _checkingDisciplineRepository.FindAsync(p => p.Id == checkingDisciplineId);
			return checkingDiscipline != null;
		}

		/// <summary>
		/// Получение всех выбранных дисциплин
		/// </summary>
		/// <returns></returns>
		public async Task<IEnumerable<CheckingDiscipline>> GetAllCheckingDisciplinesAsync()
		{
			_logger.LogInformation("Getting all checking disciplines...");
			var checkingDisciplines = await _checkingDisciplineRepository.GetAll().ToListAsync();

			_logger.LogInformation("Returning...");
			return checkingDisciplines;
		}

		/// <summary>
		/// Получение выбранной дисциплины по id
		/// </summary>
		/// <param name="checkingDisciplineId"></param>
		/// <returns></returns>
		public async Task<CheckingDiscipline> GetCheckingDisciplineAsync(int checkingDisciplineId)
		{
			_logger.LogInformation("Getting checking discipline...");
			var checkingDiscipline = await _checkingDisciplineRepository.GetAsync(checkingDisciplineId);

			_logger.LogInformation("Returning...");
			return checkingDiscipline;
		}

		/// <summary>
		/// Получение выбранных дисцплин проекта
		/// </summary>
		/// <param name="projectId"></param>
		/// <returns></returns>
		public async Task<List<CheckingDiscipline>> GetProjectCheckingDisciplinesAsync(int projectId)
		{
			_logger.LogInformation("Getting project discipline...");
			return await _checkingDisciplineRepository.GetProjectCheckingDisciplinesAsync(projectId);
		}

		/// <summary>
		/// Получение выбранных дисцплин проекта без навигации
		/// </summary>
		/// <param name="projectId"></param>
		/// <returns></returns>
		public async Task<List<CheckingDiscipline>> GetPlainProjectCheckingDisciplinesAsync(int projectId)
		{
			_logger.LogInformation("Getting project discipline...");
			return await _checkingDisciplineRepository.GetProjectCheckingDisciplinesWithoutNavigationPropertiesAsync(projectId);
		}

		/// <summary>
		/// Обновление выбранных дисциплин у проекта
		/// </summary>
		/// <param name="checkingDisciplineId"></param>
		/// <param name="update"></param>
		/// <returns></returns>
		public async Task UpdateCheckingDisciplineAsync(int checkingDisciplineId, CheckingDiscipline update)
		{
			_logger.LogInformation("Updating checking discipline...");
			var cd = new
			{
				update.MidCerificationResultsPath,
				update.MsFormsPath,
				update.TxtTestFormPath,
				update.QuestionsCount
			};
			await _checkingDisciplineRepository.UpdateAsync(checkingDisciplineId, cd);
			_logger.LogInformation("Updated!");
		}

		/// <summary>
		/// Обновление выбранных компетенций у дисциплин
		/// </summary>
		/// <param name="checkingDisciplineId"></param>
		/// <param name="competences"></param>
		/// <returns></returns>
		public async Task UpdateCheckingDisciplineCheckingCompetencesAsync(int checkingDisciplineId, List<Competence> competences)
		{
			_logger.LogInformation("Updating checking discipline's checking competences...");
			await _checkingDisciplineRepository.UpdateCheckingCompetences(checkingDisciplineId, competences);
			_logger.LogInformation("Updated!");
		}

		/// <summary>
		/// Выбор критериев оценивания
		/// </summary>
		/// <param name="checkingDisciplineId"></param>
		/// <param name="markCriteria"></param>
		/// <returns></returns>
		public async Task SetMarkCriteria(int checkingDisciplineId, List<MarkCriterion> markCriteria)
		{
			_logger.LogInformation("Updating mark criteria...");
			await _checkingDisciplineRepository.SetMarkCriteriaCompetences(checkingDisciplineId, markCriteria);
			_logger.LogInformation("Updated!");
		}

		/// <summary>
		/// Получение критериев оценивания
		/// </summary>
		/// <param name="checkingDisciplineId"></param>
		/// <returns></returns>
		public async Task<List<MarkCriterion>> GetMarkCriteria(int checkingDisciplineId)
		{
			_logger.LogInformation("Getting mark criteria...");
			return await _checkingDisciplineRepository.GetMarkCriteria(checkingDisciplineId);
		}
	}
}