using Microsoft.EntityFrameworkCore;
using ResidualKnowledgeTestApp.Server.Repositories.Common;
using ResidualKnowledgeTestApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResidualKnowledgeTestApp.Server.Repositories
{
    public class CheckingDisciplineRepository : CrudRepository<CheckingDiscipline>, ICheckingDisciplineRepository
    {
        public CheckingDisciplineRepository(ProjectContext context)
            : base(context)
        {

        }

        public async Task UpdateCheckingCompetences(int checkingDisciplineId, List<Competence> competences)
        {
            try
            {
                var discipline = await GetWithUserSelectionAsync(checkingDisciplineId);
                discipline.UserSelection.RemoveAll(us => us.CheckingDisciplineId == checkingDisciplineId);
                discipline.UserSelection.AddRange(competences.Select(c => new UserSelection { CheckingCompetenceId = c.Id, CheckingDisciplineId = checkingDisciplineId }));
                Context.Set<CheckingDiscipline>().Update(discipline);
                await Context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                var m = e.Message;
            }
        }

        public async Task SetMarkCriteriaCompetences(int checkingDisciplineId, List<MarkCriterion> markCriteria)
        {
            try
            {
                var discipline = await GetWithCriteriaAsync(checkingDisciplineId);
                if (discipline.MarkCriteria.Count == 0)
                {
                    discipline.MarkCriteria.AddRange(markCriteria);
                }
                else
                {
                    foreach (var mc in discipline.MarkCriteria)
                    {
                        var criterion = markCriteria.First(c => c.ECTSMark == mc.ECTSMark);
                        mc.MinScore = criterion.MinScore;
                        mc.MaxScore = criterion.MaxScore;
                    }
                }
                Context.Set<CheckingDiscipline>().Update(discipline);
                await Context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                var m = e.Message;
            }
        }

        private async Task<CheckingDiscipline> GetWithCriteriaAsync(int checkingDisciplineId)
            => await Context.Set<CheckingDiscipline>()
                .Include(cd => cd.MarkCriteria)
                .FirstOrDefaultAsync(cd => cd.Id == checkingDisciplineId);

        public async Task<List<CheckingDiscipline>> GetProjectCheckingDisciplinesAsync(int projectId)
        {
            return await Context.Set<CheckingDiscipline>()
                .Include(cc => cc.UserSelection)
                    .ThenInclude(us => us.CheckingCompetence)
                .Include(cd => cd.Discipline)
                    .ThenInclude(d => d.DisciplineCompetences)//
                    .ThenInclude(dc => dc.Competence)//
                .Include(cd => cd.MarkCriteria)
                .Where(cd => cd.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<List<CheckingDiscipline>> GetProjectCheckingDisciplinesWithoutNavigationPropertiesAsync(int projectId)
        {
            return await Context.Set<CheckingDiscipline>()
                .Where(cd => cd.ProjectId == projectId)
                .ToListAsync();
        }

        public async Task<CheckingDiscipline> GetWithUserSelectionAsync(int checkingDisciplineId)
        {
            return await Context.Set<CheckingDiscipline>()
                .Include(cd => cd.UserSelection)
                .FirstOrDefaultAsync(cd => cd.Id == checkingDisciplineId);
        }

        public async Task<List<MarkCriterion>> GetMarkCriteria(int checkingDisciplineId)
        {
            var cd = await Context.Set<CheckingDiscipline>()
                .Include(cd => cd.MarkCriteria)
                .FirstOrDefaultAsync(cd => cd.Id == checkingDisciplineId);

            return cd.MarkCriteria;
        }
    }
}