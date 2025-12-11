namespace FileStoring.Api.Services
{
    public class FileStorageService : IFileStorageService
    {
        /// <summary>
        /// Путь к папке, где лежат все файлы
        /// </summary>
        private readonly string _rootPath;

        /// <summary>
        /// Конструктор FileStorageService
        /// </summary>
        /// <param name="configuration">Конфигуратор, который находит переменные окружения</param>
        public FileStorageService(IConfiguration configuration)
        {
            //ищет путь в переменном окружении
            string? configuredPath = configuration["FileStorage:BasePath"];

            _rootPath = string.IsNullOrWhiteSpace(configuredPath) ? Path.Combine(AppContext.BaseDirectory, "AppFiles") : configuredPath;

            Directory.CreateDirectory(_rootPath);
        }

        /// <summary>
        /// Метод для сохранения в файле и возвращения его ID
        /// </summary>
        /// <param name="file">Файл, который нужно сохранить</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>fileId работы</returns>
        public async Task<string> SaveFileAsync(IFormFile file, CancellationToken ct = default)
        {
            if (file == null || file.Length == 0)
            {
                throw new InvalidOperationException("Файл пуст");
            }

            //генерирует уникальный id
            string fileId = Guid.NewGuid().ToString("N");

            //формирует путь
            string fullPath = Path.Combine(_rootPath, fileId);

            //создаёт файл и записывает туда
            using (FileStream stream = new FileStream(fullPath, FileMode.CreateNew, FileAccess.Write))
            {
                await file.CopyToAsync(stream, ct);
            }

            return fileId;
        }

        /// <summary>
        /// Метод для получения текста файла по ID
        /// </summary>
        /// <param name="fileId">fileId работы</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Текст файла по ID</returns>
        public Task<Stream?> GetFileStreamAsync(string fileId, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(fileId))
            {
                return Task.FromResult<Stream?>(null);
            }

            //находим путь
            string fullPath = Path.Combine(_rootPath, fileId);

            if (!File.Exists(fullPath))
            {
                return Task.FromResult<Stream?>(null);
            }

            //возвращаем текст
            FileStream stream = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            return Task.FromResult<Stream?>(stream);
        }
    }
}
