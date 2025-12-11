using FileStoring.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileStoring.Api.Controllers
{
    /// <summary>
    /// Класс контроллера для обращений к FileStoringApi
    /// </summary>
    [ApiController]
    [Route("files")]
    public class FilesController : ControllerBase
    {
        /// <summary>
        /// Сервис для работы с файлом
        /// </summary>
        private readonly IFileStorageService _fileStorageService;

        /// <summary>
        /// Конструктор FilesController
        /// </summary>
        /// <param name="fileStorageService">Сервис для работы с файлом</param>
        public FilesController(IFileStorageService fileStorageService)
        {
            _fileStorageService = fileStorageService;
        }

        /// <summary>
        /// Метод загрузки файла с ограничением по размерам
        /// </summary>
        /// <param name="file">Файл, который нужно сохранить</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Ответ от сервера с ID работы</returns>
        [HttpPost]
        [RequestSizeLimit(50_000_000)]
        public async Task<ActionResult<object>> UploadFile(
            IFormFile? file,
            CancellationToken ct)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Файл пуст");
            }

            //сохраняем файл
            string fileId = await _fileStorageService.SaveFileAsync(file, ct);

            //возвращаем с fileId
            return Ok(new
            {
                fileId
            });
        }

        /// <summary>
        /// Получение файла по fieldId
        /// </summary>
        /// <param name="fileId">ID работы в хранилище</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns></returns>
        [HttpGet("{fileId}")]
        public async Task<IActionResult> GetFile(
            string fileId,
            CancellationToken ct)
        {
            Stream? stream = await _fileStorageService.GetFileStreamAsync(fileId, ct);

            if (stream == null)
            {
                return NotFound();
            }

            //ответ в виде текста файла
            return File(stream, "text/plain");
        }
    }
}