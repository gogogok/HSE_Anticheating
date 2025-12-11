using System.Text.Json;
using FileAnalysis.Core.Domain.Entities;

namespace FileAnalysis.Infrastructure.Persistence
{
    /// <summary>
    /// Класс, заменяющий настоящую БД
    /// </summary>
    public class JsonDatabase
    {
        /// <summary>
        /// Путь к файлу, где хранятся все работы
        /// </summary>
        private readonly string _worksPath;
        
        /// <summary>
        /// Путь к файлу, где хранятся все отчёты
        /// </summary>
        private readonly string _reportsPath;

        /// <summary>
        /// Серилизатор, чтобы сделать JSON красивым 
        /// </summary>
        private readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true
        };

        /// <summary>
        /// Конструктор JsonDatabase
        /// </summary>
        /// <param name="basePath">Путь к папке</param>
        public JsonDatabase(string basePath)
        {
            //если папки нет, создаём
            Directory.CreateDirectory(basePath);
            _worksPath = Path.Combine(basePath, "works.json");
            _reportsPath = Path.Combine(basePath, "reports.json");
        }

        /// <summary>
        /// Возвращает список работ из хранилища
        /// </summary>
        /// <returns> Список работ из хранилища</returns>
        public List<Work> LoadWorks()
        {
            if (!File.Exists(_worksPath))
            {
                return new List<Work>();
            }

            string json = File.ReadAllText(_worksPath);
            //Превращает JSON в список объектов
            return JsonSerializer.Deserialize<List<Work>>(json) ?? new List<Work>();
        }

        /// <summary>
        /// Сохраняет актуальный список работ в базу
        /// </summary>
        /// <param name="works">Список работ</param>
        public void SaveWorks(List<Work> works)
        {
            string json = JsonSerializer.Serialize(works, _options);
            File.WriteAllText(_worksPath, json);
        }

        /// <summary>
        /// Возвращает список отчётов из хранилища
        /// </summary>
        /// <returns>Список отчётов</returns>
        public List<Report> LoadReports()
        {
            if (!File.Exists(_reportsPath))
            {
                return new List<Report>();
            }

            string json = File.ReadAllText(_reportsPath);
            //Превращает JSON в список объектов
            return JsonSerializer.Deserialize<List<Report>>(json) ?? new List<Report>();
        }

        /// <summary>
        /// Сохраняет в хранилище актуальный список отчётов
        /// </summary>
        /// <param name="reports"></param>
        public void SaveReports(List<Report> reports)
        {
            string json = JsonSerializer.Serialize(reports, _options);
            File.WriteAllText(_reportsPath, json);
        }
    }
}