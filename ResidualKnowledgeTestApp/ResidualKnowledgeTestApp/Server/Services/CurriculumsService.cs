using Microsoft.EntityFrameworkCore;
using ResidualKnowledgeTestApp.Server.Repositories;
using ResidualKnowledgeTestApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ResidualKnowledgeTestApp.Server.Services
{
    public class CurriculumsService : ICurriculumsService
    {
        private ICurriculumRepository _curriculumsRepository;

        public CurriculumsService(ICurriculumRepository curriculumsRepository)
        {
            _curriculumsRepository = curriculumsRepository;
        }

        public async Task<int> CreateCurriculumAsync(Curriculum curriculum)
        {
            var id = await _curriculumsRepository.AddAsync(curriculum);
            return id;
        }

        public async Task DeleteCurriculumAsync(int curriculumId)
        {
            await _curriculumsRepository.DeleteAsync(curriculumId);
        }

        public void Detach(Curriculum curric)
        {
            _curriculumsRepository.Detach(curric);
        }

        public async Task<bool> DoesCurriculumExist(int curriculumId)
        {
            var curriculum = await _curriculumsRepository.FindAsync(c => c.Id == curriculumId);
            return curriculum != null;
        }

        public async Task<List<Competence>> GetAllCurriculumCompetencesAsync(int curriculumId)
        {
            var curriculum = await _curriculumsRepository.GetWithDisciplinesAsync(curriculumId);
            return curriculum.Competences;
        }

        public async Task<List<Discipline>> GetAllCurriculumDisciplinesAsync(int curriculumId)
        {
            var curriculum = await _curriculumsRepository.GetWithDisciplinesAsync(curriculumId);
            return curriculum.Disciplines;
        }

        public async Task<List<Curriculum>> GetAllCurriculumsWithDisciplinesAsync()
        {
            var curriculums = await _curriculumsRepository.GetAllWithDisciplines();
            return curriculums;
        }

        public async Task<Curriculum> GetCurriculumWithDisciplinesAsync(int curriculumId)
        {
            var curriculum = await _curriculumsRepository.GetWithDisciplinesAsync(curriculumId);
            return curriculum;
        }

        public async Task<List<Curriculum>> GetFilteredCurriculums(Expression<Func<Curriculum, bool>> p)
        {
            return await _curriculumsRepository.GetFilteredCurriculums(p);
        }

        public async Task UpdateCurriculumAsync(int curriculumId, Curriculum update)
        {
            await _curriculumsRepository.UpdateAsync(curriculumId,
                new Curriculum()
                {
                    Disciplines = update.Disciplines,
                    Code = update.Code,
                    ProgrammeCode = update.ProgrammeCode,
                    ProgrammeName = update.ProgrammeName,
                    LevelOfEducation = update.LevelOfEducation// dopisat' nado
                });
        }
    }
}
