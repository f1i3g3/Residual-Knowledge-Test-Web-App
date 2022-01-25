using Microsoft.EntityFrameworkCore;
using ResidualKnowledgeTestApp.Server.Repositories.Common;
using ResidualKnowledgeTestApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ResidualKnowledgeTestApp.Server.Repositories
{
    public class CurriculumRepository : CrudRepository<Curriculum>, ICurriculumRepository
    {
        public CurriculumRepository(ProjectContext context) 
            : base(context)
        {

        }

        public async Task<List<Curriculum>> GetAllWithDisciplines()
        {
            return await Context.Set<Curriculum>()
                .AsNoTracking()
                .Include(c => c.Competences)
                .Include(c => c.Disciplines)
                    .ThenInclude(d => d.Competences)
                .ToListAsync();            
        }

        public async Task<List<Curriculum>> GetFilteredCurriculums(Expression<Func<Curriculum, bool>> p)
        {
            return await Context.Set<Curriculum>()
                .AsNoTracking()
                .Include(c => c.Competences)
                .Include(c => c.Disciplines)
                    .ThenInclude(d => d.Competences)
                .Where(p)
                .ToListAsync();
        }

        public async Task<Curriculum> GetWithDisciplinesAsync(int curriculumId)
        {
            return await Context.Set<Curriculum>()
                 .AsNoTracking()
                .Include(c => c.Competences)
                .Include(c => c.Disciplines)
                    .ThenInclude(d => d.Competences)
                .FirstOrDefaultAsync(c => c.Id == curriculumId);
        }
    }
}