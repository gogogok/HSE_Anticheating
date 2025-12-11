using Gateway.Api.Models;

namespace Gateway.Api.Services
{
    /// <summary>
    /// Класс для анализа текстовых файлов
    /// </summary>
    public class FileAnalysisApiClient : IFileAnalysisApiClient
    {
        /// <summary>
        /// HTTP-клиент, на который будут делаться запросы
        /// </summary>
        private readonly HttpClient _http;

        /// <summary>
        /// Конструктор FileAnalysisApiClient
        /// </summary>
        /// <param name="http"> HTTP-клиент, на который будут делаться запросы</param>
        public FileAnalysisApiClient(HttpClient http)
        {
            _http = http;
        }

        /// <summary>
        /// Возвращает Dto с информацией о работе
        /// </summary>
        /// <param name="studentId">ID студента, загрузившего работу</param>
        /// <param name="assignmentId">ID задания</param>
        /// <param name="fileId">ID по которому файл можно будет достать из хранилища</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Dto работы</returns>
        public async Task<WorkResponseDto> CreateAndAnalyzeAsync(
            string studentId,
            string assignmentId,
            string fileId,
            CancellationToken ct)
        {
            object request = new
            {
                studentId,
                assignmentId,
                fileId
            };

            HttpResponseMessage response = await _http.PostAsJsonAsync("/internal/works", request, ct);

            response.EnsureSuccessStatusCode();

            WorkResponseDto? result =
                await response.Content.ReadFromJsonAsync<WorkResponseDto>(cancellationToken: ct);

            if (result == null)
            {
                throw new InvalidOperationException("Нет ответа от FileAnalysisApi.");
            }

            return result;
        }

        /// <summary>
        /// Метод для получения всех работ студента
        /// </summary>
        /// <param name="studentId">ID студента, анализ на чьи работы мы хотим получить</param>
        /// <param name="assignmentId">ID темы, на которую мы хотим получить анализ</param>
        /// <param name="onlyPlagiarism">Параметр наличия/отсутствия плагиата</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Список работ</returns>
        public async Task<List<WorkResponseDto>> GetWorksAsync(
            string studentId,
            string assignmentId,
            bool? onlyPlagiarism,
            CancellationToken ct)
        {
            List<string> query = new List<string>();

            if (!string.IsNullOrWhiteSpace(studentId))
            {
                query.Add("studentId=" + Uri.EscapeDataString(studentId));
            }

            if (!string.IsNullOrWhiteSpace(assignmentId))
            {
                query.Add("assignmentId=" + Uri.EscapeDataString(assignmentId));
            }

            if (onlyPlagiarism.HasValue)
            {
                string value = onlyPlagiarism.Value ? "true" : "false";
                query.Add("onlyPlagiarism=" + value);
            }

            string url = "/internal/works";
            if (query.Count > 0)
            {
                url += "?" + string.Join("&", query);
            }

            HttpResponseMessage response = await _http.GetAsync(url, ct);

            if (!response.IsSuccessStatusCode)
            {
                string errorBody = await response.Content.ReadAsStringAsync(ct);
                return new List<WorkResponseDto>(); 
            }

            
            List<WorkResponseDto>? result =
                await _http.GetFromJsonAsync<List<WorkResponseDto>>(url, ct);

            return result ?? new List<WorkResponseDto>();
        }

        /// <summary>
        /// Метод для получения анализов одной работы
        /// </summary>
        /// <param name="workId">ID работы студента, по которой нужно получить анализы</param>
        /// <param name="ct">Токен, позволяющий прервать асинхронную операцию</param>
        /// <returns>Список анализов</returns>
        public async Task<List<ReportResponseDto>> GetReportsAsync(
            Guid workId,
            CancellationToken ct)
        {
            string url = "/internal/works/" + workId.ToString("D") + "/reports";

            List<ReportResponseDto>? result =
                await _http.GetFromJsonAsync<List<ReportResponseDto>>(url, ct);

            return result ?? new List<ReportResponseDto>();
        }
    }
}
