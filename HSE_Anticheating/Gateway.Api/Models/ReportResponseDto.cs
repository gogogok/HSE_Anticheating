using System;

namespace Gateway.Api.Models
{
    /// <summary>
    /// Dto отчёта об анализе на плагиат
    /// </summary>
    public class ReportResponseDto
    {
        /// <summary>
        /// ID отчёта
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// ID работы
        /// </summary>
        public Guid WorkId { get; set; }
        
        /// <summary>
        /// Время создания отчёта
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// Является ли плагиатом
        /// </summary>
        public bool IsPlagiarism { get; set; }
        
        /// <summary>
        /// Процент совпадения с другими работами
        /// </summary>
        public double SimilarityScore { get; set; }
        
        /// <summary>
        /// Ссылка на облако слов
        /// </summary>
        public string WordCloudUrl { get; set; } = null!;
    }
}