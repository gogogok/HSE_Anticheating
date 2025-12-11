using FileAnalysis.Core.Application.Interfaces;
using FileAnalysis.Core.Application.Models;
using FileAnalysis.Core.Domain.Entities;
using FileAnalysis.Core.Domain.Models;
using FileAnalysis.Core.Domain.Services;
using System.Security.Cryptography;

namespace FileAnalysis.Core.Application.Services
{
    /// <summary>
    /// Сервис по работе с работами студентов
    /// </summary>
    public class WorkService : IWorkService
    {
        private readonly IWorkRepository _workRepository;
        private readonly IReportRepository _reportRepository;
        private readonly IFileContentProvider _fileContentProvider;
        private readonly IPlagiarismDetector _plagiarismDetector;
        private readonly IWordCloudGenerator _wordCloudGenerator;

        /// <summary>
        /// Конструктор WorkService
        /// </summary>
        /// <param name="workRepository">Хранилище работ</param>
        /// <param name="reportRepository">Хранилище отчётов</param>
        /// <param name="fileContentProvider">Провайдер для получения текстов работ</param>
        /// <param name="plagiarismDetector">Антиплагиат</param>
        /// <param name="wordCloudGenerator">Генератор облака слов</param>
        public WorkService(
            IWorkRepository workRepository,
            IReportRepository reportRepository,
            IFileContentProvider fileContentProvider,
            IPlagiarismDetector plagiarismDetector,
            IWordCloudGenerator wordCloudGenerator)
        {
            _workRepository = workRepository;
            _reportRepository = reportRepository;
            _fileContentProvider = fileContentProvider;
            _plagiarismDetector = plagiarismDetector;
            _wordCloudGenerator = wordCloudGenerator;
        }

        /// <summary>
        /// Сохранение и анализ работ
        /// </summary>
        /// <param name="command">Команда для создания сущности работы</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Сущность работы</returns>
        public async Task<Work> CreateAndAnalyzeAsync(CreateWorkCommand command, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(command.StudentId) ||
                string.IsNullOrWhiteSpace(command.AssignmentId) ||
                string.IsNullOrWhiteSpace(command.FileId))
            {
                throw new ArgumentException("StudentId, AssignmentId, FileId не определены");
            }

            //считываем текст
            string rawText = await _fileContentProvider.GetFileTextAsync(command.FileId, ct);
            if (string.IsNullOrWhiteSpace(rawText))
            {
                throw new InvalidOperationException("Файл пуст");
            }

            //нормализация
            string normalizedText = Normalize(rawText);

            //хэш
            string textHash = ComputeHash(normalizedText);

            //создание Work
            Work work = new Work(Guid.NewGuid(), command.StudentId, command.AssignmentId, command.FileId, DateTime.UtcNow, textHash);

            //получаем предыдущие работы
            List<Work> prev = await _workRepository.GetPreviousForAssignmentAsync(command.AssignmentId, command.StudentId, ct);

            //анализ
            PlagiarismResult result = await _plagiarismDetector.AnalyzeAsync(work, prev);

            //генерация облака слов
            string wordCloudUrl = await _wordCloudGenerator.GenerateWordCloudUrlAsync(normalizedText);

            //отчёт
            Report report = new Report(Guid.NewGuid(), work.Id, DateTime.UtcNow, result.IsPlagiarism, result.SimilarityScore, wordCloudUrl);

            work.AddReport(report);

            //сохраняем в БД
            await _workRepository.AddAsync(work, ct);
            await _reportRepository.AddAsync(report, ct);

            return work;
        }
        

        /// <summary>
        /// Получение всех работ по фильтру
        /// </summary>
        /// <param name="filter">Фильтр</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Список работ</returns>
        public Task<List<Work>> GetAllWorksAsync(WorkFilter filter, CancellationToken ct = default)
        {
            return _workRepository.GetAllAsync(filter, ct);
        }

       /// <summary>
       /// Метод нормализауии тектса
       /// </summary>
       /// <param name="text">Текст, который нужно нормализовать</param>
       /// <returns>Нормализованный текст</returns>
        private static string Normalize(string text)
        {
            string lower = text.ToLowerInvariant();
            string cleaned = new string(lower.Select(c => char.IsLetterOrDigit(c) ? c : ' ').ToArray());
            return string.Join(' ', cleaned.Split(' ', StringSplitOptions.RemoveEmptyEntries));
        }

        /// <summary>
        /// Метод приведения текста к хешу
        /// </summary>
        /// <param name="normalizedText">Нормализованный текст</param>
        /// <returns>Посчитанный хеш</returns>
        private static string ComputeHash(string normalizedText)
        {
            using SHA256 sha = SHA256.Create();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(normalizedText);
            return Convert.ToHexString(sha.ComputeHash(bytes));
        }

        /// <summary>
        /// Создание словаря слов и их количества
        /// </summary>
        /// <param name="text">Текст, который нужно разбить на словарь</param>
        /// <returns>Словарь слов и их количества</returns>
        private static Dictionary<string, int> CountWordFreq(string text)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            foreach (string word in text.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                dict[word] = dict.TryGetValue(word, out int c) ? c + 1 : 1;
            }
            return dict;
        }
    }
}
