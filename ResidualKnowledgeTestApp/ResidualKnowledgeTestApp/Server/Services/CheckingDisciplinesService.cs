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

        public async Task DeleteCheckingDisciplineAsync(int checkingDisciplineId)
        {
            await _checkingDisciplineRepository.DeleteAsync(checkingDisciplineId);
        }

        public async Task<bool> DoesCheckingDisciplineExist(int checkingDisciplineId)
        {
            var checkingDiscipline = await _checkingDisciplineRepository.FindAsync(p => p.Id == checkingDisciplineId);
            return checkingDiscipline != null;
        }

        public async Task<IEnumerable<CheckingDiscipline>> GetAllCheckingDisciplinesAsync()
        {
            var checkingDisciplines = await _checkingDisciplineRepository.GetAll().ToListAsync();
            return checkingDisciplines;
        }

        public async Task<CheckingDiscipline> GetCheckingDisciplineAsync(int checkingDisciplineId)
        {
            var checkingDiscipline = await _checkingDisciplineRepository.GetAsync(checkingDisciplineId);
            return checkingDiscipline;
        }

        public async Task<List<CheckingDiscipline>> GetProjectCheckingDisciplinesAsync(int projectId)
        {
            return await _checkingDisciplineRepository.GetProjectCheckingDisciplinesAsync(projectId);
        }

        public async Task<List<CheckingDiscipline>> GetPlainProjectCheckingDisciplinesAsync(int projectId)
        {
            return await _checkingDisciplineRepository.GetProjectCheckingDisciplinesWithoutNavigationPropertiesAsync(projectId);
        }

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

        public async Task UpdateCheckingDisciplineCheckingCompetencesAsync(int checkingDisciplineId, List<Competence> competences)
        {
            await _checkingDisciplineRepository.UpdateCheckingCompetences(checkingDisciplineId, competences);
        }

        public async Task SetMarkCriteria(int checkingDisciplineId, List<MarkCriterion> markCriteria)
        {
            await _checkingDisciplineRepository.SetMarkCriteriaCompetences(checkingDisciplineId, markCriteria);
        }

        public async Task<List<MarkCriterion>> GetMarkCriteria(int checkingDisciplineId)
        {
            return await _checkingDisciplineRepository.GetMarkCriteria(checkingDisciplineId);
        }
    }
}