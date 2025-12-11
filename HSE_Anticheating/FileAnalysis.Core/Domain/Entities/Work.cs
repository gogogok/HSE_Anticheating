namespace FileAnalysis.Core.Domain.Entities
{
    /// <summary>
    /// Сущность работы
    /// </summary>
    public class Work
    {
        public Guid Id { get; private set; }
        public string StudentId { get; private set; } = default!;
        public string AssignmentId { get; private set; } = default!;
        public string FileId { get; private set; } = default!;
        public DateTime SubmittedAt { get; private set; }
        public string TextHash { get; private set; } = default!;
        
        /// <summary>
        /// Список отчётов по работе
        /// </summary>
        public List<Report> Reports { get; private set; } = new();

        private Work() { }

        /// <summary>
        /// Конструктор Work
        /// </summary>
        /// <param name="id">ID работы</param>
        /// <param name="studentId">ID студента</param>
        /// <param name="assignmentId">ID студента</param>
        /// <param name="fileId">ID работы в хранилище</param>
        /// <param name="submittedAt">Время загрузки работы</param>
        /// <param name="textHash">Хеш работы</param>
        public Work(Guid id, string studentId, string assignmentId, string fileId, DateTime submittedAt, string textHash)
        {
            Id = id;
            StudentId = studentId;
            AssignmentId = assignmentId;
            FileId = fileId;
            SubmittedAt = submittedAt;
            TextHash = textHash;
        }

        /// <summary>
        /// Метод добавления отчёта по работе 
        /// </summary>
        /// <param name="report">Отчёт по работе</param>
        public void AddReport(Report report)
        {
            Reports.Add(report);
        }
    }
}