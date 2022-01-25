using Microsoft.EntityFrameworkCore;
using ResidualKnowledgeTestApp.Server.Repositories.Common;
using ResidualKnowledgeTestApp.Shared;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System;

namespace ResidualKnowledgeTestApp.Server.Repositories
{
    public class ProjectsRepository : CrudRepository<Project>, IProjectsRepository
    {
        public ProjectsRepository(ProjectContext context)
            : base(context)
        {

        }

        public async Task<Project> GetWithCheckingDisciplinesWithDisciplineAsync(int projectId)
        {
            return await Context.Set<Project>()
               .AsNoTracking()
               .Include(p => p.Curriculum)
                .Include(p => p.CheckingDisciplines)
                    .ThenInclude(cd => cd.Discipline)
                .FirstOrDefaultAsync(p => p.Id == projectId);
        }

        public async Task<List<Project>> GetAllWithCurriculumAsync()
        {
            return await Context.Set<Project>()
               .AsNoTracking()
               .Include(p => p.Curriculum).ToListAsync();
        }

        public async Task<Project> GetWithEverythingAsync(int projectId)
        {
                return await Context.Set<Project>()
               .AsNoTracking()
               .Include(p => p.Curriculum)
               .ThenInclude(c => c.Disciplines)
                .Include(p => p.CheckingDisciplines)
                    .ThenInclude(c => c.UserSelection)
                    .ThenInclude(cd => cd.CheckingCompetence)
                .Include(p => p.CheckingDisciplines)
                    .ThenInclude(cd => cd.Discipline)
                    .ThenInclude(d => d.DisciplineCompetences)
                    .ThenInclude(dc => dc.Competence)
                .Include(p => p.CheckingDisciplines)
                .ThenInclude(cd => cd.MarkCriteria)
                //.Include(p => p.User)
                //.Include(p => p.CheckingDisciplines)
                //    .ThenInclude(cd => cd.Discipline)
                //    .ThenInclude(d => d.Competences)
                .FirstOrDefaultAsync(p => p.Id == projectId);
        }
    }
}
