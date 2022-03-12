using ContingentParser;
using CurriculumParser;
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
			/*
			try
			{
				string path = AppDomain.CurrentDomain.BaseDirectory;

				DocxCurriculum curriculum = new DocxCurriculum(path + ""); // файл берется с сервера, нужна структура файлов

				//var contingent = await _projectsRepository.GetWithEverythingAsync(); // файл загружается через отдельный контроллер
				Contingent contingent = new Contingent(path + "списки студентов мат обес.xls"); // файл берется с сервера, нужна структура файлов
				var groups = contingent
					.Where(s => s.CurriculumCode == curriculum.CurriculumCode.Replace("/", "\\"))
					.Select(s => s.GroupInContingent)
					.Distinct()
					.ToList();

				var user = new ResidualKnowledgeConsoleApp.User("Кузнецов", "Дмитрий", "Владимирович"); // здесь должен быть автор ответов - по идее, нужна авторизация/вносить самому
				var config = new ResidualKnowledgeConsoleApp.MsFormsParserConfiguration(4, 8, 3, user); // взял, как в примере

				var checkingDisciplines = await GetProjectCheckingDisciplinesAsync(projectId);
				var consoleAppDisciplines = new List<ResidualKnowledgeConsoleApp.CheckingDiscipline>();

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
						foreach(var c in iComp)
                        {
							c.SingleOrDefault(x => x.Code == d.CheckingCompetences.Code);
                        }
						//
                    }

					foreach (var c in d.CheckingCompetences)
					{
						var consoleCC = curriculum.Competences.SingleOrDefault(x => x.Code == c.Code);
						consoleCheckCompet.Add(consoleCC);
					}

					var listOfMarks = new List<ResidualKnowledgeConsoleApp.MarkCriterion>(); // откуда?

					var consoleDiscp = new ResidualKnowledgeConsoleApp.CheckingDiscipline(curriculumDiscp, consoleCheckCompet, user, scale: listOfMarks,
						config: config); // тестируется

					consoleAppDisciplines.Add(consoleDiscp);
				}

				var userChoice = new ResidualKnowledgeConsoleApp.UserChoice(user, curriculum, contingent, consoleAppDisciplines);
				var competenceCriterion = new List<ResidualKnowledgeConsoleApp.MarkCriterion>
				{
				new ResidualKnowledgeConsoleApp.MarkCriterion(90, 100, 'A', 5),
				new ResidualKnowledgeConsoleApp.MarkCriterion(80, 89, 'B', 4),
				new ResidualKnowledgeConsoleApp.MarkCriterion(70, 79, 'C', 4),
				new ResidualKnowledgeConsoleApp.MarkCriterion(60, 69, 'D', 3),
				new ResidualKnowledgeConsoleApp.MarkCriterion(50, 59, 'E', 3),
				new ResidualKnowledgeConsoleApp.MarkCriterion(0, 49, 'F', 2)
				 }; // это на серевер надо настроить же?

				//
				var midCertificationResult = new List<ResidualKnowledgeConsoleApp.MidCerificationAssesmentResult>();
				var studentAnswers = new List<ResidualKnowledgeConsoleApp.StudentAnswer>();
				foreach (var d in consoleAppDisciplines)
				{
					var parser = new ResidualKnowledgeConsoleApp.ResidualKnowledgeInputFilesParser.ResidualKnowledgeDataParser(d, userChoice.Students);
					var result = parser.Parse();
					midCertificationResult.AddRange(result.MidCerificationResults);
					studentAnswers.AddRange(result.StudentAnswers);
					d.Questions.AddRange(result.Questions);
				}

				link = ResidualKnowledgeConsoleApp.Generator.Generate(curriculum, contingent, consoleAppDisciplines);

				var spreadsheetGenerator = new ResidualKnowledgeConsoleApp.GoogleSpreadsheetGenerator(userChoice, groups, competenceCriterion, studentAnswers, midCertificationResult);
				spreadsheetGenerator.Generate(); // exception point

				// link = ResidualKnowledgeConsoleApp.Generator.Generate(curriculum, contingent, consoleAppDisciplines);
				//

				await _projectsRepository.UpdateSheetLink(projectId, link);

				// обновление ссылки

			}
			catch
			{
				link = null;
			}
			*/

			return link;
		}
	}
}