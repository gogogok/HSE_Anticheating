using Gateway.Api.Models;
using Gateway.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Api.Controllers
{
    /// <summary>
    /// Класс контроллера для обращений преподавателя
    /// </summary>
    [ApiController]
    [Route("teacher/works")]
    public class TeacherWorksController : ControllerBase
    {
        /// <summary>
        /// Клиент анализа
        /// </summary>
        private readonly IFileAnalysisApiClient _fileAnalysisApiClient;

        /// <summary>
        /// Конструктор TeacherWorksController
        /// </summary>
        /// <param name="fileAnalysisApiClient">Клиент анализа</param>
        public TeacherWorksController(IFileAnalysisApiClient fileAnalysisApiClient)
        {
            _fileAnalysisApiClient = fileAnalysisApiClient;
        }

        /// <summary>
        /// Метод для получения всех работ студента по get запросу
        /// </summary>
        /// <param name="studentId">Id студента, который сдал работу</param>
        /// <param name="assignmentId">ID темы, на которую мы хотим получить анализ</param>
        /// <param name="onlyPlagiarism">Параметр наличия/отсутствия плагиата</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Список работ</returns>
        [HttpGet]
        public async Task<ActionResult<List<WorkResponseDto>>> GetWorks(
            [FromQuery] string? studentId,
            [FromQuery] string? assignmentId,
            [FromQuery] bool? onlyPlagiarism,
            CancellationToken ct)
        {
            List<WorkResponseDto> works = await _fileAnalysisApiClient.GetWorksAsync(
                studentId,
                assignmentId,
                onlyPlagiarism,
                ct);

            return Ok(works);
        }

        /// <summary>
        /// Метод для получения анализов одной работы по запросу
        /// </summary>
        /// <param name="workId">ID работы студента, по которой нужно получить анализы</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Список анализов</returns>
        [HttpGet("{workId:guid}/reports")]
        public async Task<ActionResult<List<ReportResponseDto>>> GetReports(
            Guid workId,
            CancellationToken ct)
        {
            List<ReportResponseDto> reports =
                await _fileAnalysisApiClient.GetReportsAsync(workId, ct);

            return Ok(reports);
        }
    }
}