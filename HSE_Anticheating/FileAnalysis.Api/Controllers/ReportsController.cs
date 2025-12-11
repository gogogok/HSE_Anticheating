using FileAnalysis.Api.Models;
using FileAnalysis.Core.Application.Interfaces;
using FileAnalysis.Core.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FileAnalysis.Api.Controllers
{
    /// <summary>
    /// Класс контроллера для создания отчётов ою анализе
    /// </summary>
    [ApiController]
    [Route("internal/works/{workId:guid}/reports")]
    public class ReportsController : ControllerBase
    {
        /// <summary>
        /// Сервис для работы с отчётами
        /// </summary>
        private readonly IReportService _reportService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reportService">Сервис для работы с отчётами</param>
        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        /// <summary>
        /// Получение отчёта по анализу
        /// </summary>
        /// <param name="workId">ID работы</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Ответ сервера со списком отчётов</returns>
        [HttpGet]
        public async Task<ActionResult<List<ReportResponseDto>>> GetReports(
            Guid workId,
            CancellationToken ct)
        {
            //получаем работы
            List<Report> reports = await _reportService.GetReportsForWorkAsync(workId, ct);

            List<ReportResponseDto> result = new List<ReportResponseDto>();
            foreach (Report report in reports)
            {
                //превращаем в Dto
                ReportResponseDto dto = new ReportResponseDto
                {
                    Id = report.Id,
                    WorkId = report.WorkId,
                    CreatedAt = report.CreatedAt,
                    IsPlagiarism = report.IsPlagiarism,
                    SimilarityScore = report.SimilarityScore,
                    WordCloudUrl = report.WordCloudUrl
                };

                result.Add(dto);
            }
            return Ok(result);
        }
    }
}