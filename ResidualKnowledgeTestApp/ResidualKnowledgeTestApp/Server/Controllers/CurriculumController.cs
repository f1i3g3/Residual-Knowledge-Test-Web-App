using AutoMapper;
using CurriculumParser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResidualKnowledgeTestApp.Server.Services;
using ResidualKnowledgeTestApp.Shared;
using ResidualKnowledgeTestApp.Shared.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ResidualKnowledgeTestApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurriculumController : ControllerBase
    {
        private ICurriculumsService _curriculumService;
        private IWebHostEnvironment _environment;
        private ICompetenceService _competenceService;
        private IDisciplinesService _disciplinesService;
        private IProjectsService _projectsService;
        private IMapper _mapper;

        public CurriculumController(ICurriculumsService curriculumService, IWebHostEnvironment environment,
            ICompetenceService competenceService, IDisciplinesService disciplinesService, IMapper mapper, 
            IProjectsService projectsService)
        {
            _curriculumService = curriculumService;
            _environment = environment;
            _competenceService = competenceService;
            _disciplinesService = disciplinesService;
            _projectsService = projectsService;
            _mapper = mapper;
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
        public async Task<IActionResult> CreateCurriculum([FromBody] Curriculum curriculumViewModel) // добавить id пользователя
        {
            // где projectId??
            var path = Path.Combine(_environment.ContentRootPath, "Files", curriculumViewModel.FileName); // projectId??
            var parsedCurriculum = new DocxCurriculum(path);

            //var fileInfo = new FileInfo(path);
            //fileInfo.Delete();

            var curriculum = new Curriculum()
            {
                Code = parsedCurriculum.CurriculumCode,
                ProgrammeCode = parsedCurriculum.Programme.Code,
                ProgrammeName = parsedCurriculum.Programme.RussianName,
                LevelOfEducation = parsedCurriculum.Programme.LevelOfEducation,
                FileName = curriculumViewModel.FileName, 
                ProjectId = curriculumViewModel.ProjectId
            };

            var curriculumId = await _curriculumService.CreateCurriculumAsync(curriculum);
            await _competenceService.CreateCompetencesAsync(curriculumId, parsedCurriculum.Competences);

            var currentYear = DateTime.Now.Year % 100;
            var course = currentYear - int.Parse(parsedCurriculum.CurriculumCode.Substring(0, 2)); // тут неверно
            var currentSemester = DateTime.Now.Month > 0 ? 8 : 8; // тут неверно
            var filteredDisciplineImplementations = parsedCurriculum.Disciplines
                .Where(d => d.Type == DisciplineType.Base)
                .SelectMany(d => d.Implementations.Where(i => i.MonitoringTypes == "экзамен" && i.Semester < currentSemester));
            //.Where(); // + добавить не ранее чем полгода или в прошлом семестре?
            // добавить, что если дисциплины одинаковые, то выбираем реализацию с наибольшим семестром

            await _disciplinesService.CreateDisciplinesAsync(curriculumId, filteredDisciplineImplementations);
            await _projectsService.UpdateProjectAsync(curriculum.ProjectId, new { CurriculumId = curriculum.Id, Stage = Stage.DisciplinesChoosing });

            var mapped = _mapper.Map<CurriculumDTO>(curriculum);
            return Ok(mapped); //CreatedAtAction(nameof(CreateCurriculum), new { curriculumId = curriculumId }, curriculum);
        }


        // Уточнить код ниже у Ульяны

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
