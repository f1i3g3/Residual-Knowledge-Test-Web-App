using Microsoft.EntityFrameworkCore;
using ResidualKnowledgeApp.Server.Repositories.Common;
using System.Linq.Expressions;

namespace ResidualKnowledgeApp.Server.Repositories
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