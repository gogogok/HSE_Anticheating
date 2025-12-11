using System;
using System.Threading;
using System.Threading.Tasks;
using Gateway.Api.Models;
using Gateway.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Api.Controllers
{
    /// <summary>
    /// Класс контроллера для обращений студента
    /// </summary>
    [ApiController]
    [Route("student/works")]
    public class StudentWorksController : ControllerBase
    {
        /// <summary>
        /// Клиент для хранения
        /// </summary>
        private readonly IFileStoringApiClient _fileStoringApiClient;
        
        /// <summary>
        /// Клиент анализа
        /// </summary>
        private readonly IFileAnalysisApiClient _fileAnalysisApiClient;

        /// <summary>
        /// Конструктор StudentWorksController
        /// </summary>
        /// <param name="fileStoringApiClient">Клиент для хранения</param>
        /// <param name="fileAnalysisApiClient">Клиент анализа</param>
        public StudentWorksController(
            IFileStoringApiClient fileStoringApiClient,
            IFileAnalysisApiClient fileAnalysisApiClient)
        {
            _fileStoringApiClient = fileStoringApiClient;
            _fileAnalysisApiClient = fileAnalysisApiClient;
        }

        /// <summary>
        /// Метод, который отправляет запрос на хранение работы и возвращает Dto работы
        /// </summary>
        /// <param name="request">реквест, полученный от Swagger, в котором содержаться данные для работы FileStoringApi</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Dto работы студента</returns>
        [HttpPost]
        public async Task<ActionResult<WorkResponseDto>> UploadWork(
            [FromForm] UploadWorkRequest request,
            CancellationToken ct)
        {
            
            if (request.File == null || request.File.Length == 0)
            {
                // Можно вернуть 400, но с нормальным текстом:
                return BadRequest(new
                {
                    message = "Вы не прикрепили файл. Пожалуйста, выберите файл перед отправкой.",
                    code = "FILE_REQUIRED"
                });
            }

            
            string fileId = await _fileStoringApiClient.UploadFileAsync(
                request.File,
                ct);

            WorkResponseDto work = await _fileAnalysisApiClient.CreateAndAnalyzeAsync(
                request.StudentId,
                request.AssignmentId,
                fileId,
                ct);

            return Ok(work);
        }
    }
}