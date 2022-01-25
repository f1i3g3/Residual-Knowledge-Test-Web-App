using Microsoft.EntityFrameworkCore;
using ResidualKnowledgeTestApp.Server.Repositories;
using ResidualKnowledgeTestApp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResidualKnowledgeTestApp.Server.Services
{
    public class ProjectsService : IProjectsService
    {
        //private readonly IMapper _mapper;
        private readonly IProjectsRepository _projectsRepository;
        private readonly ICheckingDisciplinesService _checkingDisciplinesService;

        public ProjectsService(IProjectsRepository projectsRepository, 
            ICheckingDisciplinesService checkingDisciplinesService)
        {
            _projectsRepository = projectsRepository;
            _checkingDisciplinesService = checkingDisciplinesService;
        }

        public async Task<Project> CreateProjectAsync(Project project) // createProjectVM
        {
            //var project = _mapper.Map<Project>(projectVM);
            project.CreationTime = DateTime.Now;
            project.LastEditionTime = DateTime.Now;
            var projectId = await _projectsRepository.AddAsync(project);
            return project;
        }

        public async Task DeleteProjectAsync(int projectId)
        {
            await _projectsRepository.DeleteAsync(projectId);
        }

        public async Task<bool> DoesProjectExist(int projectId)
        {
            var project = await _projectsRepository.FindAsync(p => p.Id == projectId);
            return project != null;
        }

        public async Task<IEnumerable<Project>> GetAllProjectsAsync()
        {
            var projects = await _projectsRepository.GetAllWithCurriculumAsync();
            return projects;
        }

        public async Task<Project> GetProjectAsync(int projectId)
        {
            var project = await _projectsRepository.GetWithEverythingAsync(projectId);
            return project;
        }

        public async Task UpdateProjectAsync(int projectId, object update)
        {
            await _projectsRepository.UpdateAsync(projectId, update);
            //var project = await _projectsRepository.GetWithCheckingDisciplinesAsync(projectId);
            //await _checkingDisciplinesService.DeleteProjectCheckingDisciplines(projectId, project.CheckingDisciplines);
            //await _checkingDisciplinesService.AddProjectCheckingDisciplines(projectId, update.CheckingDisciplines);
        }
        
        public async Task<List<CheckingDiscipline>> GetProjectCheckingDisciplinesAsync(int projectId)
        {
            var project = await _projectsRepository.GetWithCheckingDisciplinesWithDisciplineAsync(projectId);
            return project.CheckingDisciplines;
        }

        public async Task<List<DisciplineCompetence>> GetProjectCompetencesForSelection(int id)
        {
            var project = await _projectsRepository.GetWithEverythingAsync(id);
            var disciplineCompetencePairs = project.CheckingDisciplines
                .SelectMany(cd => cd.Discipline.DisciplineCompetences)
                .ToList();
            return disciplineCompetencePairs;
        }
    }
}