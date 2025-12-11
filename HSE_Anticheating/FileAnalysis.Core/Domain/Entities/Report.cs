namespace FileAnalysis.Core.Domain.Entities
{
    /// <summary>
    /// Сущность отчёта
    /// </summary>
    public class Report
    {
        public Guid Id { get; private set; }
        public Guid WorkId { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public bool IsPlagiarism { get; private set; }
        public double SimilarityScore { get; private set; }
        public string? WordCloudUrl { get; private set; }

        private Report() { }

        /// <summary>
        /// Конструктор отчёта
        /// </summary>
        /// <param name="id">ID отчёта</param>
        /// <param name="workId">ID работы</param>
        /// <param name="createdAt">Время создания отчёта</param>
        /// <param name="isPlagiarism">Есть плагиат или нет</param>
        /// <param name="similarityScore">Процент схожести</param>
        /// <param name="wordCloudUrl">Ссылка на облако из слов</param>
        public Report(Guid id, Guid workId, DateTime createdAt, bool isPlagiarism, double similarityScore, string? wordCloudUrl = null)
        {
            Id = id;
            WorkId = workId;
            CreatedAt = createdAt;
            IsPlagiarism = isPlagiarism;
            SimilarityScore = similarityScore;
            WordCloudUrl = wordCloudUrl;
        }
    }
}