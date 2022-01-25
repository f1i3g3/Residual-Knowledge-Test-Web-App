using ResidualKnowledgeTestApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ResidualKnowledgeTestApp.Server.Services
{
    public interface ICompetenceService
    {
        Task<int> CreateCompetenceAsync(Competence competence);

        Task<List<Competence>> GetAllCurriculumCompetencesAsync(int curriculumId);

        Task<List<Competence>> GetFiltered(Expression<Func<Competence, bool>> p);
        
        void Detach(Competence competence);

        void DetachRange(IEnumerable<Competence> competences);

        Task CreateCompetencesAsync(int curriculumId, List<CurriculumParser.Competence> competences);
    }
}
