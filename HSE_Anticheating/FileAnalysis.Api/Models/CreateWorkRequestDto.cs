namespace FileAnalysis.Api.Models
{
    /// <summary>
    /// Dto запроса работы
    /// </summary>
    public class CreateWorkRequestDto
    {
        /// <summary>
        /// ID студента
        /// </summary>
        public string StudentId { get; set; } = null!;
        
        /// <summary>
        /// ID темы
        /// </summary>
        public string AssignmentId { get; set; } = null!;
        
        /// <summary>
        /// ID файла в хранилище
        /// </summary>
        public string FileId { get; set; } = null!;
    }
}