using ResidualKnowledgeApp.ContingentParser;
using ResidualKnowledgeApp.CurriculumParser;
using ResidualKnowledgeApp.Server.Repositories;

namespace ResidualKnowledgeApp.Server.Services
{
	public class ProjectsService : IProjectsService
	{
		//private readonly IMapper _mapper;
		private readonly IProjectsRepository _projectsRepository;
		private readonly ICheckingDisciplinesService _checkingDisciplinesService;
		private readonly IWebHostEnvironment _environment;
		private readonly ILogger<ProjectsService> _logger;

		public ProjectsService(IProjectsRepository projectsRepository,
			ICheckingDisciplinesService checkingDisciplinesService, IWebHostEnvironment environment, ILogger<ProjectsService> logger)
		{
			_projectsRepository = projectsRepository;
			_checkingDisciplinesService = checkingDisciplinesService;
			_environment = environment;
			_logger = logger;
		}

		public async Task<Project> CreateProjectAsync(Project project) // createProjectVM
		{
			_logger.LogInformation($"Creating project with id {project.Id}...");
			
			project.CreationTime = DateTime.Now;
			project.LastEditionTime = DateTime.Now;

			_logger.LogInformation(project.CurriculumId.ToString()); // Here!
			var projectId = await _projectsRepository.AddAsync(project);
			_logger.LogInformation("Created!");

			return project;
		}

		public async Task DeleteProjectAsync(int projectId)
		{
			_logger.LogInformation($"Deleting project with id {projectId}...");
			// MarkCriteria -> CheckingDisciplines
			await _projectsRepository.DeleteAsync(projectId);
			_logger.LogInformation("Deleted!");
		}

		public async Task<bool> DoesProjectExist(int projectId)
		{
			var project = await _projectsRepository.FindAsync(p => p.Id == projectId);
			return project != null;
		}

		public async Task<IEnumerable<Project>> GetAllProjectsAsync()
		{
			_logger.LogInformation("Trying to get all projects...");
			var projects = await _projectsRepository.GetAllWithCurriculumAsync();
			_logger.LogInformation("Returning...");
			return projects;
		}

		public async Task<Project> GetProjectAsync(int projectId)
		{
			_logger.LogInformation($"Trying to get project with id {projectId}...");

			var project = await _projectsRepository.GetWithEverythingAsync(projectId);
			_logger.LogInformation(project.CurriculumId.ToString());

			_logger.LogInformation("Returning...");
			return project;
		}

		public async Task UpdateProjectAsync(int projectId, object update)
		{
			_logger.LogInformation($"Updating project with id {projectId}...");
			await _projectsRepository.UpdateAsync(projectId, update);
			_logger.LogInformation("Updated!");
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
			_logger.LogInformation($"Getting sheet link for project with id {projectId}...");
			var link = await _projectsRepository.GetSheetLink(projectId);

			if (String.IsNullOrEmpty(link))
			{
				link = await GenerateLink(projectId);
			}

			_logger.LogInformation($"Returning...");
			return link;
		}

		/// <summary>
		/// Получение сгенерированной ссылки на гугл-документ
		/// </summary>
		/// <param name="projectId"></param>
		/// <returns></returns>
		private async Task<string> GenerateLink(int projectId)
		{
			string link = String.Empty;
			
			try
			{
				int adminId = 0;
				string filesPath = Path.Combine(_environment.ContentRootPath, $"ServerFiles/User_{adminId}/Project_{projectId}"); // redo with projectID

				// TODO: вписать в инструкцию правильное наименование файлов

				var curriculum = new DocxCurriculum(filesPath + "curriculum.docx");
				var contingent = new Contingent(filesPath + "contingent.xlsx");  // файл загружается с сервера через отдельный контроллер и форму
																			   
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
			}
			catch
			{
				link = String.Empty;
			}

			return link;
		}
	}
}