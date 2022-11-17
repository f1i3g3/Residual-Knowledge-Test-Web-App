using AutoMapper;
using ResidualKnowledgeApp.CurriculumParser;
using Microsoft.AspNetCore.Mvc;
using ResidualKnowledgeApp.Server.Services;
using ResidualKnowledgeApp.Shared.DTO;

namespace ResidualKnowledgeApp.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CurriculumController : ControllerBase
	{
		private readonly ICurriculumsService _curriculumService;
		private readonly IWebHostEnvironment _environment;
		private readonly ICompetenceService _competenceService;
		private readonly IDisciplinesService _disciplinesService;
		private readonly IProjectsService _projectsService;
		private readonly IMapper _mapper;
		private readonly ILogger<CurriculumController> _logger;

		public CurriculumController(ICurriculumsService curriculumService, IWebHostEnvironment environment,
			ICompetenceService competenceService, IDisciplinesService disciplinesService, IMapper mapper,
			IProjectsService projectsService, ILogger<CurriculumController> logger)
		{
			_curriculumService = curriculumService;
			_environment = environment;
			_competenceService = competenceService;
			_disciplinesService = disciplinesService;
			_projectsService = projectsService;
			_mapper = mapper;
			_logger = logger;
		}

		/// <summary>
		/// Получение всех учебных планов
		/// </summary>
		/// <returns></returns>
		[HttpGet] // фильтр на пользователя сделать потом
		[ProducesResponseType(typeof(List<Curriculum/*ProjectInfoOverviewDTO*/>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAllCurriculums()
		{
			var curriculums = await _curriculumService.GetAllCurriculumsWithDisciplinesAsync();
			return Ok(curriculums.ToList()); // (_mapper.Map<List<ProjectInfoOverviewDTO>>(projects));
		}

		/// <summary>
		/// Получение учебного плана по id
		/// </summary>
		/// <param name="id">Id учебного плана</param>
		/// <returns></returns>
		[ProducesResponseType(typeof(Curriculum/*DetailedProjectInfoDTO*/), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[HttpGet("{id}")]
		public async Task<IActionResult> GetCurriculum(int id)
		{
			var exists = await _curriculumService.DoesCurriculumExist(id);
			if (!exists)
			{
				return NotFound();
			}

			var curriculum = await _curriculumService.GetCurriculumWithDisciplinesAsync(id);
			return Ok(curriculum);// (_mapper.Map<DetailedProjectInfoDTO>(project));
		}

		/// <summary>
		/// Получение всех дисциплин
		/// </summary>
		/// <param name="id">Id учебного плана</param>
		/// <returns></returns>
		[HttpGet("disciplines/{id}")]
		[ProducesResponseType(typeof(List<DisciplineDTO>/*DetailedProjectInfoDTO*/), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetAllCurriculumDisciplines(int id)
		{
			var exists = await _curriculumService.DoesCurriculumExist(id);
			if (!exists)
			{
				return NotFound();
			}

			var disciplines = await _curriculumService.GetAllCurriculumDisciplinesAsync(id);
			var mapped = _mapper.Map<List<DisciplineDTO>>(disciplines);
			return Ok(mapped);
		}

		/// <summary>
		/// Создание учебного плана
		/// </summary>
		/// <param name="curriculumViewModel"></param>
		/// <returns></returns>
		[ProducesResponseType(typeof(CurriculumDTO), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[HttpPost]
		public async Task<IActionResult> CreateCurriculum([FromBody] Curriculum curriculumViewModel)
		{
            _logger.LogInformation($"Start CurriculumController!");
            int adminId = 0;
			var path = Path.Combine(_environment.ContentRootPath, "ServerFiles", $"User_{adminId}", $"Project_{curriculumViewModel.ProjectId}", curriculumViewModel.FileName);
			_logger.LogInformation($"Creating curriculum from {path}");

			var parsedCurriculum = new DocxCurriculum(path);
			var curriculum = new Curriculum()
			{
				Code = parsedCurriculum.CurriculumCode,
				ProgrammeCode = parsedCurriculum.Programme.Code,
				ProgrammeName = parsedCurriculum.Programme.RussianName,
				LevelOfEducation = parsedCurriculum.Programme.LevelOfEducation,
				FileName = curriculumViewModel.FileName, 
				ProjectId = curriculumViewModel.ProjectId
			};

			_logger.LogInformation("Creating on server...");
			var curriculumId = await _curriculumService.CreateCurriculumAsync(curriculum);

			_logger.LogInformation("Adding competences...");
			await _competenceService.CreateCompetencesAsync(curriculumId, parsedCurriculum.Competences);

			var currentYear = DateTime.Now.Year % 100;
			// пометки Ульяны, что немного не так
			var course = currentYear - int.Parse(parsedCurriculum.CurriculumCode.Substring(0, 2));
			var currentSemester = DateTime.Now.Month > 0 ? 8 : 8;
			//
			var filteredDisciplineImplementations = parsedCurriculum.Disciplines
				.Where(d => d.Type == DisciplineType.Base)
				.SelectMany(d => d.Implementations.Where(i => i.MonitoringTypes == "экзамен" && i.Semester < currentSemester));
			//.Where(); // + добавить не ранее чем полгода или в прошлом семестре?
			// добавить, что если дисциплины одинаковые, то выбираем реализацию с наибольшим семестром

			_logger.LogInformation("Adding disciplines");
			await _disciplinesService.CreateDisciplinesAsync(curriculumId, filteredDisciplineImplementations);

            _logger.LogInformation("Updating choosed...");
            await _projectsService.UpdateProjectAsync(curriculum.ProjectId, new { CurriculumId = curriculum.Id, Stage = Stage.DisciplinesChoosing });

			var mapped = _mapper.Map<CurriculumDTO>(curriculum);
            _logger.LogInformation($"Curriculum with id {curriculumId} created!");

            return Ok(mapped); // CreatedAtAction(nameof(CreateCurriculum), new { curriculumId = curriculumId }, curriculum);
		}



		//var filteredCurriculums = await _curriculumService.GetFilteredCurriculums(c => c.Code == parsedCurriculum.CurriculumCode);
		//var exists = null != filteredCurriculums.FirstOrDefault();
		//if (exists)
		//{
		//    return Ok(filteredCurriculums.FirstOrDefault().Id);
		//}

		//// здесь компетенции загружаются в память
		//var competences = await _curriculumService.GetAllCurriculumCompetencesAsync(curriculumId);
		//// здесь среди загруженных выбираюся еще и так же загружаются в память, с такими же id
		//var disciplineCompetences = d.Implementations.SelectMany(i => i.Competences).Distinct()
		//    .Select(c => competences.FirstOrDefault(comp => comp.Code == c.Code))
		//    .ToList();

		//Competences = competences
		//_competenceService.Detach(model);
		//_disciplinesService.Detach(discipline);
		//_competenceService.DetachRange(competences);

		//discipline.Competences.AddRange(disciplineCompetences); 
		// возможно стоит сделать это с помощью _disciplinesService update

		/// <summary>
		/// Удаление учебного плана
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		[ProducesResponseType(typeof(Curriculum/*DetailedProjectInfoDTO*/), StatusCodes.Status200OK)]
		public async Task<ActionResult> DeleteCurriculum(int id)
		{
			var exists = await _curriculumService.DoesCurriculumExist(id);
			if (!exists)
			{
				return NotFound();
			}

			await _curriculumService.DeleteCurriculumAsync(id);
			return Ok(await _curriculumService.GetAllCurriculumsWithDisciplinesAsync());
		}
	}
}
