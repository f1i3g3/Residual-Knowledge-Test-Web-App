using Microsoft.EntityFrameworkCore;
using ResidualKnowledgeTestApp.Server.Repositories.Common;
using ResidualKnowledgeTestApp.Shared;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ResidualKnowledgeTestApp.Server.Repositories
{
    public class DisciplineRepository : CrudRepository<Discipline>, IDisciplineRepository
    {
        public DisciplineRepository(ProjectContext context) 
            : base(context)
        {
            
        }

        public async Task<Discipline> GetWithCompetencesAsync(int disciplineId)
        {
            var r = await Context.Set<Discipline>()
                .AsNoTracking()
                .Include(d => d.Competences)
                //.Include(d => d.DisciplineCompetences)
                //.Include(d => d.Curriculum)
                .FirstOrDefaultAsync(d => d.Id == disciplineId);

            return r;
        }

        public async Task AddCompetences(int disciplineId, List<Competence> competences)
        {
            var discipline = await GetAsync(disciplineId);
            discipline.Competences.AddRange(competences);
            Context.Set<Discipline>().Update(discipline);
            await Context.SaveChangesAsync();
        }
    }
}
