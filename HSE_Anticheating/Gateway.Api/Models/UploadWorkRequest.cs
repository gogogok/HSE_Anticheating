namespace Gateway.Api.Models
{
    /// <summary>
    /// Модель для загрузки работы на сервер
    /// </summary>
    public class UploadWorkRequest
    {
        /// <summary>
        /// ID студента
        /// </summary>
        public string StudentId { get; set; } = string.Empty;

        /// <summary>
        /// ID темы
        /// </summary>
        public string AssignmentId { get; set; } = string.Empty;

        /// <summary>
        /// Файл работы
        /// </summary>
        public IFormFile? File { get; set; }
    }
}