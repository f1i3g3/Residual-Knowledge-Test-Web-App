using Microsoft.EntityFrameworkCore;
using ResidualKnowledgeApp.Server.Repositories;
using System.Linq.Expressions;

namespace ResidualKnowledgeApp.Server.Services
{
    public class CompetenceService : ICompetenceService
    {
        private ICompetenceRepository _competenceRepository;

        public CompetenceService(ICompetenceRepository competenceRepository)
        {
            _competenceRepository = competenceRepository;
        }


        public async Task CreateCompetencesAsync(int curriculumId, List<CurriculumParser.Competence> competences)
        {
            foreach (var c in competences)
            {
                var competence = new Competence
                {
                    Code = c.Code,
                    Description = c.Description,
                    CurriculumId = curriculumId
                };
                await CreateCompetenceAsync(competence);
                Detach(competence); // а почему эти штуки вообще в контексте?
            }
        }

        public async Task<int> CreateCompetenceAsync(Competence competence)
        {
            var id = await _competenceRepository.AddAsync(competence);
            return id;
        }

        public void Detach(Competence competence)
        {
            _competenceRepository.Detach(competence);
        }

        public void DetachRange(IEnumerable<Competence> competences)
        {
            _competenceRepository.DetachRange(competences);
        }

        public async Task<List<Competence>> GetAllCurriculumCompetencesAsync(int curriculumId)
        {
            var competences = await _competenceRepository.GetAll().ToListAsync();
            return competences;
        }

        public async Task<List<Competence>> GetFiltered(Expression<Func<Competence, bool>> p)
        {
            return await _competenceRepository.GetFilteredCompetences(p);
        }
    }
}
