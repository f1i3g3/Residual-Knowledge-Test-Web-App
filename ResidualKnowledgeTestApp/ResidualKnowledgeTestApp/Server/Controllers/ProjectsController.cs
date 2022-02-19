using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using ResidualKnowledgeTestApp.Shared;
using ResidualKnowledgeTestApp.Server.Services;
using System.Linq;
using AutoMapper;
using ResidualKnowledgeTestApp.Shared.DTO;
using System;
using ResidualKnowledgeTestApp.Shared.ViewModels;

namespace ResidualKnowledgeTestApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProjectsService _projectsService;
        private ICheckingDisciplinesService _checkingDisciplinesService;
        private ICompetenceService _competenceService;
        private IUserService _userService;

        public ProjectsController(IMapper mapper, ICompetenceService competenceService,
            IProjectsService projectsService, ICheckingDisciplinesService checkingDisciplinesService,
            IUserService userService)
        {
            _competenceService = competenceService;
            _mapper = mapper;
            _checkingDisciplinesService = checkingDisciplinesService;
            _projectsService = projectsService;
            _userService = userService;
        }

        /// <summary>
        /// Получение всех проектов
        /// </summary>
        /// <returns></returns>
        [HttpGet] // фильтр потом
        [ProducesResponseType(typeof(List<ProjectOverviewDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _projectsService.GetAllProjectsAsync();
            var mapped = _mapper.Map<List<ProjectOverviewDTO>>(projects);
            return Ok(mapped);
        }

        /// <summary>
        /// Получение проекта по id
        /// </summary>
        /// <param name="id">Id проекта</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(ProjectDetailsDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProject(int id)
        {
            var exists = await _projectsService.DoesProjectExist(id);
            if (!exists)
            {
                return NotFound();
            }

            var project = await _projectsService.GetProjectAsync(id);
            var mapped = _mapper.Map<ProjectDetailsDTO>(project);
            return Ok(mapped);
        }

        /// <summary>
        /// Создание проекта
        /// </summary>
        /// <param name="projectVM"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateProject([FromBody] CreateProjectVM projectVM) // добавить id пользователя
        {
            var createModel = _mapper.Map<Project>(projectVM);
            var created = await _projectsService.CreateProjectAsync(createModel);
            //return CreatedAtAction(nameof(CreateProject), new { projectId = id }, projectVM);
            var mapped = _mapper.Map<ProjectDetailsDTO>(created);
            return Ok(mapped);
        }

        /// <summary>
        /// Обновление данных проекта
        /// </summary>
        /// <param name="id"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]  // добавить ограничение доступа
        public async Task<IActionResult> UpdateProject(int id, Project update)
        {
            var exists = await _projectsService.DoesProjectExist(id);
            if (!exists)
            {
                return NotFound();
            }

            await _projectsService.UpdateProjectAsync(id, update);
            return NoContent();
        }

        /// <summary>
        /// Удаление проекта
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Project/*DetailedProjectInfoDTO*/), StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteProject(int id)
        {
            var exists = await _projectsService.DoesProjectExist(id);
            if (!exists)
            {
                return NotFound();
            }

            await _projectsService.DeleteProjectAsync(id);
            return Ok(await _projectsService.GetAllProjectsAsync());
        }

        /// <summary>
        /// Получение выбранных дисциплин проекта
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("disciplines/{id}")] // фильтр потом
        [ProducesResponseType(typeof(List<CheckingDisciplineDetailsDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProjectCheckingDiscipline(int id)
        {
            var checkingDisciplines = await _checkingDisciplinesService.GetProjectCheckingDisciplinesAsync(id);
            var mapped = _mapper.Map<List<CheckingDisciplineDetailsDTO>>(checkingDisciplines.ToList());
            return Ok(mapped);
        }

        /// <summary>
        /// Выбор дисциплин проекта
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="disciplines"></param>
        /// <returns></returns>
        [HttpPut("disciplines/set/{projectId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]  // добавить ограничение доступа
        public async Task<IActionResult> SetCheckingDisciplinesOfProject(int projectId, [FromBody] List<DisciplineDTO> disciplines)
        {
            var exists = await _projectsService.DoesProjectExist(projectId);
            if (!exists)
            {
                return NotFound();
            }

            var checkingDisciplines = await _checkingDisciplinesService.GetProjectCheckingDisciplinesAsync(projectId);
            var disciplinesMarkedAsCheckingIds = checkingDisciplines.Select(cd => cd.Discipline.Id);
            var disciplineToMarkAsCheckingIds = disciplines.Select(d => d.Id);
            var disciplinesToSetAsUnchecking = checkingDisciplines.Where(cd => !disciplineToMarkAsCheckingIds.Contains(cd.DisciplineId));
            var disciplinesToSetAsChecking = disciplines.Where(d => !disciplinesMarkedAsCheckingIds.Contains(d.Id));

            foreach (var cd in disciplinesToSetAsUnchecking)
            {
                await _checkingDisciplinesService.DeleteCheckingDisciplineAsync(cd.Id);
            }

            var mappedDisciplines = disciplinesToSetAsChecking.Select(d => new CheckingDiscipline { DisciplineId = d.Id, ProjectId = projectId }).ToList();
            foreach (var d in mappedDisciplines)
            {
                await _checkingDisciplinesService.CreateCheckingDisciplineAsync(d);
            }

            await _projectsService.UpdateProjectAsync(projectId, new { Stage = Stage.CompetencesChoosing });

            checkingDisciplines = await _checkingDisciplinesService.GetProjectCheckingDisciplinesAsync(projectId);
            var mapped = _mapper.Map<List<CheckingDisciplineDetailsDTO>>(checkingDisciplines.ToList());
            return Ok(mapped);
        }

        /// <summary>
        /// Получение выбранных компетенций проекта
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("competences/{id}")] // фильтр потом
        [ProducesResponseType(typeof(List<CompetenceWithDisciplineDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProjectCheckingCompetences(int id) // не вызывается
        {
            var checkingDisciplines = await _projectsService.GetProjectCheckingDisciplinesAsync(id);
            var userSelections = checkingDisciplines.SelectMany(cd => cd.UserSelection).ToList();
            return Ok(_mapper.Map<List<CompetenceWithDisciplineDTO>>(userSelections));
        }

        /// <summary>
        /// Выбор компетенций проекта
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="selectedCompetences"></param>
        /// <returns></returns>
        [HttpPut("competences/set/{projectId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]  // добавить ограничение доступа
        public async Task<IActionResult> SetCheckingCompetences(int projectId, [FromBody] List<CompetenceWithDisciplineDTO> selectedCompetences)
        {
            var exists = await _projectsService.DoesProjectExist(projectId);
            if (!exists)
            {
                return NotFound();
            }
            var engagedDisciplines = selectedCompetences.Select(c => c.Discipline.Id).Distinct();
            var checkingDisciplines = await _checkingDisciplinesService.GetPlainProjectCheckingDisciplinesAsync(projectId);
            var engagedCheckingDisciplines = checkingDisciplines.Where(cd => engagedDisciplines.Contains(cd.DisciplineId)).ToList();
            foreach (var cd in engagedCheckingDisciplines)
            {
                var mapped = _mapper.Map<List<Competence>>(selectedCompetences.Where(sc => sc.Discipline.Id == cd.DisciplineId).ToList());
                await _checkingDisciplinesService.UpdateCheckingDisciplineCheckingCompetencesAsync(cd.Id, mapped);
                _competenceService.DetachRange(mapped);
            }

            await _projectsService.UpdateProjectAsync(projectId, new { Stage = Stage.FilesUploading });

            return Ok();
        }

        /// <summary>
        /// Получение компетенций проекта для выборки
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("competences/selection/{id}")] // фильтр потом
        [ProducesResponseType(typeof(List<CompetenceWithDisciplineDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProjectCompetencesForSelection(int id)
        {
            var checkingDisciplines = await _checkingDisciplinesService.GetProjectCheckingDisciplinesAsync(id);
            var disciplineCompetencePairs = checkingDisciplines.SelectMany(cd => cd.Discipline.DisciplineCompetences).ToList();
            var mapped = _mapper.Map<List<CompetenceWithDisciplineDTO>>(disciplineCompetencePairs);
            return Ok(mapped);
        }
        // await _projectsService.UpdateProjectAsync(curriculum.ProjectId, new { CurriculumId = curriculum.Id, Stage = Stage.DisciplinesChoosing });

        /// <summary>
        /// Выбор информации пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPut("userinfo/set")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SetUserInfo([FromBody] User user)
        {
            var exists = await _projectsService.DoesProjectExist(user.ProjectId);
            if (!exists)
            {
                return NotFound();
            }
            if (user.Id > 0)
            {
                var update = new
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Patronymic = user.Patronymic,
                    RightAnswersEmail = user.RightAnswersEmail
                };
                await _userService.UpdateUserAsync(user.Id, update);
            }
            else
            {
                await _userService.CreateUserAsync(user);
            }
            return Ok();
        }

        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<IActionResult> GetUserInfo(int projectId)
        //{
        //    var exists = await _projectsService.DoesProjectExist(projectId);
        //    if (!exists)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(await _userService.GetProjectUserAsync(projectId));
        //}
    }
}
