using FileAnalysis.Api.Models;
using FileAnalysis.Core.Application.Interfaces;
using FileAnalysis.Core.Application.Models;
using FileAnalysis.Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FileAnalysis.Api.Controllers
{
    /// <summary>
    /// Класс контроллера для работы со студенческими работами
    /// </summary>
    [ApiController]
    [Route("internal/works")]
    public class WorksController : ControllerBase
    {
        /// <summary>
        /// Сервис для работы со студенческими работами
        /// </summary>
        private readonly IWorkService _workService;

        /// <summary>
        /// Конструктор WorksController
        /// </summary>
        /// <param name="workService"> Сервис для работы со студенческими работами</param>
        public WorksController(IWorkService workService)
        {
            _workService = workService;
        }

        /// <summary>
        /// Создание работы
        /// </summary>
        /// <param name="request">Реквест, который нужно превратить в работу</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Dto работы</returns>
        [HttpPost]
        public async Task<ActionResult<WorkResponseDto>> CreateWork(
            [FromBody] CreateWorkRequestDto request,
            CancellationToken ct)
        {
            //создание команды
            CreateWorkCommand command = new CreateWorkCommand
            {
                StudentId = request.StudentId,
                AssignmentId = request.AssignmentId,
                FileId = request.FileId
            };

            //создание сущности работы
            Work work = await _workService.CreateAndAnalyzeAsync(command, ct);

            //делаем из сущности Dto
            WorkResponseDto dto = ToWorkResponse(work);
            
            //возвращаем готовый ресурс
            return CreatedAtAction(nameof(GetWorkById), new { id = work.Id }, dto);
        }

        /// <summary>
        /// Метод получения работы по ID
        /// </summary>
        /// <param name="id">ID работы</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Ответ от api с работой студента</returns>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<WorkResponseDto>> GetWorkById(
            Guid id,
            CancellationToken ct)
        {
            //ищем по ID
            List<Work> all = await _workService.GetAllWorksAsync(
                new WorkFilter(),
                ct);

            //ищем по ID
            Work? work = all.Find(w => w.Id == id);
            if (work == null)
            {
                return NotFound();
            }

            //делаем из сущности Dto
            WorkResponseDto dto = ToWorkResponse(work);
            return Ok(dto);
        }

        /// <summary>
        /// Метод получения всех работ по критериям
        /// </summary>
        /// <param name="studentId">ID студента</param>
        /// <param name="assignmentId">ID темы</param>
        /// <param name="onlyPlagiarism">Наличие плагиата</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Ответ от api со списком работ</returns>
        [HttpGet]
        public async Task<ActionResult<List<WorkResponseDto>>> GetWorks(
            [FromQuery] string? studentId,
            [FromQuery] string? assignmentId,
            [FromQuery] bool? onlyPlagiarism,
            CancellationToken ct)
        {
            //фильтр для выборки работ
            WorkFilter filter = new WorkFilter
            {
                StudentId = studentId,
                AssignmentId = assignmentId,
                OnlyPlagiarism = onlyPlagiarism
            };

            List<Work> works = await _workService.GetAllWorksAsync(filter, ct);

            List<WorkResponseDto> result = new List<WorkResponseDto>();
            foreach (Work work in works)
            {
                //делаем из сущности Dto
                result.Add(ToWorkResponse(work));
            }

            return Ok(result);
        }

        /// <summary>
        /// Метод приведения из сущности Work в Dto WorkResponseDto
        /// </summary>
        /// <param name="work">Сущность, которую нужно превратить</param>
        /// <returns>Итоговый WorkResponseDto</returns>
        private static WorkResponseDto ToWorkResponse(Work work)
        {
            bool hasPlagiarism = false;
            
            //проверяем есть ли в отчётах сведения о плагиате
            if (work.Reports.Count > 0)
            {
                foreach (Report report in work.Reports)
                {
                    if (report.IsPlagiarism)
                    {
                        hasPlagiarism = true;
                        break;
                    }
                }
            }

            WorkResponseDto dto = new WorkResponseDto
            {
                Id = work.Id,
                StudentId = work.StudentId,
                AssignmentId = work.AssignmentId,
                FileId = work.FileId,
                SubmittedAt = work.SubmittedAt,
                HasPlagiarism = hasPlagiarism
            };

            return dto;
        }
    }
}
