using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ResidualKnowledgeTestApp.Server.Repositories;
using ResidualKnowledgeTestApp.Shared;
using ResidualKnowledgeTestApp.Shared.DTO;
using ResidualKnowledgeTestApp.Shared.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ResidualKnowledgeTestApp.Server.Services
{
    public class CheckingDisciplinesService : ICheckingDisciplinesService
    {
        private readonly IMapper _mapper;
        private readonly ICheckingDisciplineRepository _checkingDisciplineRepository;
        private IMarkCriterionRepository _markCriterionRepository;

        public CheckingDisciplinesService(IMapper mapper, ICheckingDisciplineRepository checkingDisciplineRepository,
            IMarkCriterionRepository markCriterionRepository)
        {
            _mapper = mapper;
            _checkingDisciplineRepository = checkingDisciplineRepository;
            _markCriterionRepository = markCriterionRepository;
        }

        /// <summary>
        /// Выбор дисциплин
        /// </summary>
        /// <param name="checkingDiscipline"></param>
        /// <returns></returns>
        public async Task<int> CreateCheckingDisciplineAsync(CheckingDiscipline checkingDiscipline) // createProjectVM
        {
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

            return checkingDisciplineId;
        }

        /// <summary>
        /// Отмена выбора дисциплин
        /// </summary>
        /// <param name="checkingDisciplineId"></param>
        /// <returns></returns>
        public async Task DeleteCheckingDisciplineAsync(int checkingDisciplineId)
        {
            await _checkingDisciplineRepository.DeleteAsync(checkingDisciplineId);
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
            var checkingDisciplines = await _checkingDisciplineRepository.GetAll().ToListAsync();
            return checkingDisciplines;
        }

        /// <summary>
        /// Получение выбранной дисциплины по id
        /// </summary>
        /// <param name="checkingDisciplineId"></param>
        /// <returns></returns>
        public async Task<CheckingDiscipline> GetCheckingDisciplineAsync(int checkingDisciplineId)
        {
            var checkingDiscipline = await _checkingDisciplineRepository.GetAsync(checkingDisciplineId);
            return checkingDiscipline;
        }

        /// <summary>
        /// Получение выбранных дисцплин проекта
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<List<CheckingDiscipline>> GetProjectCheckingDisciplinesAsync(int projectId)
        {
            return await _checkingDisciplineRepository.GetProjectCheckingDisciplinesAsync(projectId);
        }

        /// <summary>
        /// Получение выбранных дисцплин проекта без навигации
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        public async Task<List<CheckingDiscipline>> GetPlainProjectCheckingDisciplinesAsync(int projectId)
        {
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
            var cd = new
            {
                MidCerificationResultsPath = update.MidCerificationResultsPath,
                MsFormsPath = update.MsFormsPath,
                TxtTestFormPath = update.TxtTestFormPath,
                QuestionsCount = update.QuestionsCount
            };
            await _checkingDisciplineRepository.UpdateAsync(checkingDisciplineId, cd);
        }

        /// <summary>
        /// Обновление выбранных компетенций у дисциплин
        /// </summary>
        /// <param name="checkingDisciplineId"></param>
        /// <param name="competences"></param>
        /// <returns></returns>
        public async Task UpdateCheckingDisciplineCheckingCompetencesAsync(int checkingDisciplineId, List<Competence> competences)
        {
            await _checkingDisciplineRepository.UpdateCheckingCompetences(checkingDisciplineId, competences);
        }

        /// <summary>
        /// Выбор критериев оценивания
        /// </summary>
        /// <param name="checkingDisciplineId"></param>
        /// <param name="markCriteria"></param>
        /// <returns></returns>
        public async Task SetMarkCriteria(int checkingDisciplineId, List<MarkCriterion> markCriteria)
        {
            await _checkingDisciplineRepository.SetMarkCriteriaCompetences(checkingDisciplineId, markCriteria);
        }

        /// <summary>
        /// Получение критериев оценивания
        /// </summary>
        /// <param name="checkingDisciplineId"></param>
        /// <returns></returns>
        public async Task<List<MarkCriterion>> GetMarkCriteria(int checkingDisciplineId)
        {
            return await _checkingDisciplineRepository.GetMarkCriteria(checkingDisciplineId);
        }

        public async Task<string> GetGeneratedSheet(int checkingDisciplineId) // здесь должны быть параметры? userChoice ??
        {
            // checkingDisciplines?? где вообще данные хранятся?
            //List<ResidualKnowledgeConsoleApp.MarkCriterion> competenceCriterion = _markCriterionRepository; // файл ??
            //List<string> groups = null; // здесь нужен контингент, которого нет
            //ResidualKnowledgeConsoleApp.UserChoice userChoice = new ResidualKnowledgeConsoleApp.UserChoice(user, groups, null, null); // контингент ??
            //List<ResidualKnowledgeConsoleApp.StudentAnswer> studentAnswers = null;
            //List<ResidualKnowledgeConsoleApp.MidCerificationAssesmentResult> midCertificationResult = MidCerificationResultsPath;

            //var spreadsheetGenerator = new ResidualKnowledgeConsoleApp.GoogleSpreadsheetGenerator(userChoice, groups, competenceCriterion, studentAnswers, midCertificationResult);
            throw new System.NotImplementedException();
            //await _checkingDisciplineRepository. //TODO: типы данных в БД??
            //TODO: Figma + письма
        }
    }
}