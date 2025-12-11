namespace Gateway.Api.Models
{
    /// <summary>
    /// DTO работы студента
    /// </summary>
    public class WorkResponseDto
    {
        /// <summary>
        /// ID работы
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// ID студента
        /// </summary>
        public string StudentId { get; set; } = null!;
        /// <summary>
        /// ID темы
        /// </summary>
        public string AssignmentId { get; set; } = null!;
        /// <summary>
        /// ID работы в базе
        /// </summary>
        public string FileId { get; set; } = null!;
        /// <summary>
        /// Момент загрузки работы
        /// </summary>
        public DateTime SubmittedAt { get; set; }
        /// <summary>
        /// Обнаружен ли плагиат
        /// </summary>
        public bool HasPlagiarism { get; set; }
    }
}