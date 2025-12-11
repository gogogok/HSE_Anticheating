using Gateway.Api.Services;

namespace Gateway.Api.DependencyInjection
{
    /// <summary>
    /// Класс, который регистрирует клиенты в DI-контейнере
    /// </summary>
    public static class GatewayServicesExtensions
    {
        /// <summary>
        ///  Метод, который регистрирует клиенты в DI-контейнере
        /// </summary>
        /// <param name="services">Сервис, в который будут зарегистрированы клиенты</param>
        /// <param name="configuration">Конфиг, который нужно прочитать</param>
        /// <returns>Тот же сервер, который был передан</returns>
        public static IServiceCollection AddGatewayServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            //ищет путь порта в переменных окружения
            string? fileAnalysisUrl = configuration["FileAnalysisApi:Url"];
            if (string.IsNullOrWhiteSpace(fileAnalysisUrl))
            {
                fileAnalysisUrl = "http://localhost:5002";
            }

            string? fileStoringUrl = configuration["FileStoringApi:Url"];
            if (string.IsNullOrWhiteSpace(fileStoringUrl))
            {
                fileStoringUrl = "http://localhost:5001";
            }

            //регистрация HTTP-клиентов
            services.AddHttpClient<IFileAnalysisApiClient, FileAnalysisApiClient>(client =>
            {
                client.BaseAddress = new Uri(fileAnalysisUrl);
            });

            services.AddHttpClient<IFileStoringApiClient, FileStoringApiClient>(client =>
            {
                client.BaseAddress = new Uri(fileStoringUrl);
            });

            return services;
        }
    }
}