using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ResidualKnowledgeTestApp.Server.Services;
using ResidualKnowledgeTestApp.Shared;
using ResidualKnowledgeTestApp.Shared.DTO;
using ResidualKnowledgeTestApp.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResidualKnowledgeTestApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckingDisciplinesController : ControllerBase // protected class & methods??
    {
        private ICheckingDisciplinesService _checkingDisciplinesService;
        private IMapper _mapper;

        public CheckingDisciplinesController(ICheckingDisciplinesService checkingDisciplinesService, IMapper mapper)
        {
            _checkingDisciplinesService = checkingDisciplinesService;
            _mapper = mapper;
        }

        /// <summary>
        /// Добавление выбранной дисциплины
        /// </summary>
        /// <param name="disciplineVM"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)] // ??
        [HttpPost]
        public async Task<IActionResult> CreateCheckingDiscipline([FromBody] CheckingDiscipline disciplineVM) // добавить id пользователя
        {
            var checkingDisciplineId = await _checkingDisciplinesService.CreateCheckingDisciplineAsync(disciplineVM);
            return Ok(checkingDisciplineId); //CreatedAtAction(nameof(CreateCurriculum), new { curriculumId = curriculumId }, curriculum);
        }

        /// <summary>
        /// Обновление выбранной дисциплины
        /// </summary>
        /// <param name="checkingDisciplineId"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        [HttpPut("{checkingDisciplineId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]  // добавить ограничение доступа
        public async Task<IActionResult> UpdateCheckingDiscipline(int checkingDisciplineId, [FromBody] CheckingDiscipline update)
        {
            var exists = await _checkingDisciplinesService.DoesCheckingDisciplineExist(checkingDisciplineId);
            if (!exists)
            {
                return NotFound();
            }

            await _checkingDisciplinesService.UpdateCheckingDisciplineAsync(checkingDisciplineId, update);
            return NoContent();
        }

        /// <summary>
        /// Удаление выбора дисциплины
        /// </summary>
        /// <param name="checkingDisciplineId">Id дисциплины</param>
        /// <returns></returns>
        [HttpDelete("{checkingDisciplineId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(List<CheckingDisciplineDetailsDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteCheckingDiscipline(int checkingDisciplineId)
        {
            var exists = await _checkingDisciplinesService.DoesCheckingDisciplineExist(checkingDisciplineId);
            if (!exists)
            {
                return NotFound();
            }

            await _checkingDisciplinesService.DeleteCheckingDisciplineAsync(checkingDisciplineId);
            return Ok(await _checkingDisciplinesService.GetAllCheckingDisciplinesAsync());
        }

        /// <summary>
        /// Обновление компетенций выбранной дисциплины
        /// </summary>
        /// <param name="checkingDisciplineId"></param>
        /// <param name="disciplineCompetences"></param>
        /// <returns></returns>
        [HttpPut("competences/{checkingDisciplineId}")]
        public async Task<ActionResult> UpdateCheckingDisciplineCompetences(int checkingDisciplineId, [FromBody] List<CompetenceWithDisciplineDTO> disciplineCompetences)
        {
            var exists = await _checkingDisciplinesService.DoesCheckingDisciplineExist(checkingDisciplineId);
            if (!exists)
            {
                return NotFound();
            }
            var competences = _mapper.Map<List<Competence>>(disciplineCompetences);
            await _checkingDisciplinesService.UpdateCheckingDisciplineCheckingCompetencesAsync(checkingDisciplineId, competences);
            return Ok();
        }

        /// <summary>
        /// Выбор критериев оценивания
        /// </summary>
        /// <param name="checkingDisciplineId"></param>
        /// <param name="markCriteria"></param>
        /// <returns></returns>
        [HttpPut("criteria/{checkingDisciplineId}")]
        public async Task<ActionResult> SetMarkCriteria(int checkingDisciplineId, [FromBody] List<MarkCriterion> markCriteria)
        {
            var exists = await _checkingDisciplinesService.DoesCheckingDisciplineExist(checkingDisciplineId);
            if (!exists)
            {
                return NotFound();
            }
            await _checkingDisciplinesService.SetMarkCriteria(checkingDisciplineId, markCriteria);
            return Ok();
        }

        /// <summary>
        /// Получение критериев оценивания
        /// </summary>
        /// <param name="checkingDisciplineId"></param>
        /// <returns></returns>
        [HttpGet("criteria/{checkingDisciplineId}")]
        public async Task<ActionResult> GetMarkCriteria(int checkingDisciplineId)
        {
            var exists = await _checkingDisciplinesService.DoesCheckingDisciplineExist(checkingDisciplineId);
            if (!exists)
            {
                return NotFound();
            }
            await _checkingDisciplinesService.GetMarkCriteria(checkingDisciplineId);
            return Ok();
        }

        /// <summary>
        /// Получение сгенерированной ссылки на гугл-документ
        /// </summary>
        /// <param name="checkingDisciplineId"></param>
        /// <returns></returns>
        [HttpGet("criteria/{checkingDisciplineId}")]
        public async Task<ActionResult> GetGeneratedSheet(int checkingDisciplineId) // TODO: перенести в проекты/отдельный контроллер!!
        {
            var exists = await _checkingDisciplinesService.DoesCheckingDisciplineExist(checkingDisciplineId); // redo
            if (!exists)
            {
                return NotFound();
            }
            await _checkingDisciplinesService.GetGeneratedSheet(checkingDisciplineId);
            return Ok();
        }
    }
}
