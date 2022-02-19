using ResidualKnowledgeTestApp.Shared;
using ResidualKnowledgeTestApp.Shared.DTO;
using ResidualKnowledgeTestApp.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ResidualKnowledgeTestApp.Client.Services
{
    public interface ICheckingDisciplinesService
    {
        event Action OnChange;

        List<ProjectOverviewDTO> Projects { get; }

        Task<List<ProjectOverviewDTO>> GetProjectsAsync();

        Task<List<ProjectOverviewDTO>> DeleteProject(int id);

        ///////////////////////////////////////////////////////

        UserChoice UserChoice { get; }

        ProjectDetailsDTO Project { get; }

        CurriculumDTO Curriculum { get; }

        List<CheckingDisciplineDetailsDTO> CheckingDisciplines { get; }

        List<DisciplineDTO> DisciplinesForSelection { get; }

        HashSet<CompetenceWithDisciplineDTO> CompetencesForSelection { get; }

        HashSet<CompetenceWithDisciplineDTO> SelectedCompetences { get; }

        Task SaveMarkCriteria(int checkingDisciplineId, List<MarkCriterion> markCriteria);

        Task<ProjectDetailsDTO> CreateProject(CreateProjectVM project);

        Task LoadProject(int projectId);

        Task<List<ProjectOverviewDTO>> UpdateProject(Project project, int id);

        Task<CurriculumDTO> UploadCurriculumAsync(Curriculum curriculum, HttpContent content);
        
        Task SetCheckingDisciplines(IEnumerable<DisciplineDTO> selectedDisciplines);

        Task SetCheckingCompetences(IEnumerable<CompetenceWithDisciplineDTO> selectedCompetences);

        Task UpdateCheckingDisciplineFiles(int id, CheckingDisciplineDetailsDTO updated);

        Task UploadFileAsync(HttpContent content);

        Task GetCheckingDisciplines();

        Task GenerateLink();
    }
}
