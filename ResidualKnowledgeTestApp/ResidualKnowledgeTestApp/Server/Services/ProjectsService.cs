using ContingentParser;
using CurriculumParser;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using ResidualKnowledgeTestApp.Server.Repositories;
using ResidualKnowledgeTestApp.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ResidualKnowledgeTestApp.Server.Services
{
	public class ProjectsService : IProjectsService
	{
		//private readonly IMapper _mapper;
		private readonly IProjectsRepository _projectsRepository;
		private readonly ICheckingDisciplinesService _checkingDisciplinesService;
		private readonly IWebHostEnvironment _environment;

		public ProjectsService(IProjectsRepository projectsRepository,
			ICheckingDisciplinesService checkingDisciplinesService, IWebHostEnvironment environment)
		{
			_projectsRepository = projectsRepository;
			_checkingDisciplinesService = checkingDisciplinesService;
			_environment = environment;
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

		public async Task<string> GetSheetLink(int projectId)
		{
			var link = await _projectsRepository.GetSheetLink(projectId);

			if (link == null)
            {
				link = await GenerateLink(projectId);
            }

			return link;
		}

		private async Task<string> GenerateLink(int projectId)
		{
			string link = null;
			
			try
			{
				string filesPath = AppDomain.CurrentDomain.BaseDirectory + $"../../../Files/"; // redo with projectID
				// Path.Combine(_environment.ContentRootPath, "Files", $"Project_{projectId}_Files");
				// структура? перименование файлов/вписать в инструкцию? очистка ненужных?

				DocxCurriculum curriculum = new DocxCurriculum(filesPath + "curriculum.docx");
				// файл берется с сервера - нужна структура файлов/путь из бд
				Contingent contingent = new Contingent(filesPath + "contingent.xlsx");  // файл загружается с сервера
																	// через отдельный контроллер, нужна структура файлов
				var groups = contingent
					.Where(s => s.CurriculumCode == curriculum.CurriculumCode.Replace("/", "\\"))
					.Select(s => s.GroupInContingent)
					.Distinct()
					.ToList();

				var checkingDisciplines = await GetProjectCheckingDisciplinesAsync(projectId);
				var consoleAppDisciplines = new List<ConsoleApp.CheckingDiscipline>();

				foreach (var d in checkingDisciplines)
				{
					var curriculumDiscp = curriculum.Disciplines.SingleOrDefault(cd => cd.Code == d.Discipline.Code);

					// var test = d.CheckingCompetences.Select(cc => curriculum.Competences.Contains(cc));
					// var consoleCheckCompet = d.CheckingCompetences.Select(cc => d.CheckingCompetences.Contains(cc)); // curriculumD => выбрать компетенции, соответствующие checkingCompetencies

					List<CurriculumParser.Competence> consoleCheckCompet = null;

					// var algebraCompetences = algebra.Implementations[0].Competences.ToList();
					var curriculumDiscpImpl = curriculumDiscp.Implementations;
					foreach (var i in curriculumDiscpImpl)
                    {
						//
						var iComp = i.Competences;

						/*
						foreach(var c in iComp)
                        {
							c.SingleOrDefault(x => x.Code == d.CheckingCompetences.Code);
                        }
						*/
						//
                    }

					foreach (var c in d.CheckingCompetences)
					{
						var consoleCC = curriculum.Competences.SingleOrDefault(x => x.Code == c.Code);
						consoleCheckCompet.Add(consoleCC);
					}

					// сопоставление компетенций с сервера и файла
					var listOfMarks = new List<ConsoleApp.MarkCriterion>(); // с сервера

					var consoleDiscp = new ConsoleApp.CheckingDiscipline(curriculumDiscp, consoleCheckCompet, null, scale: listOfMarks);
					consoleAppDisciplines.Add(consoleDiscp);
				}

				link = "https://docs.google.com/spreadsheets/d/" + ConsoleApp.Generator.Generate(curriculum, contingent, consoleAppDisciplines);
				await _projectsRepository.UpdateSheetLink(projectId, link);

				// обновление ссылки

			}
			catch
			{
				link = null;
			}

			return link;
		}
	}
}