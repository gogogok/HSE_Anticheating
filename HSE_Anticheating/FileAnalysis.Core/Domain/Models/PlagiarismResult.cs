namespace FileAnalysis.Core.Domain.Models
{
    /// <summary>
    /// Модель результата работы антиплагиата
    /// </summary>
    public class PlagiarismResult
    {
        /// <summary>
        /// Есть ли плагиат
        /// </summary>
        public bool IsPlagiarism { get; }
        
        /// <summary>
        /// Процент плагиата
        /// </summary>
        public double SimilarityScore { get; }
        
        /// <summary>
        /// ID работ, с которыми совпала проверенная работы
        /// </summary>
        public IReadOnlyList<Guid> MatchedWorkIds { get; }

        /// <summary>
        /// Конструктор PlagiarismResult
        /// </summary>
        /// <param name="isPlagiarism">Если ли плагиат</param>
        /// <param name="similarityScore">Процент схожести</param>
        ///  <param name="matchedWorkIds">ID работ, которые совпали с проверенной</param>
        public PlagiarismResult(bool isPlagiarism, double similarityScore, IReadOnlyList<Guid>? matchedWorkIds = null)
        {
            IsPlagiarism = isPlagiarism;
            SimilarityScore = similarityScore;
            MatchedWorkIds = matchedWorkIds ?? [];
        }

        /// <summary>
        /// Возврат отрицательного результата плагиата, если его не обнаружено
        /// </summary>
        /// <returns>Результат плагиата</returns>
        public static PlagiarismResult NoPlagiarism()
        {
            return new PlagiarismResult(false, 0);
        }
    }
}
