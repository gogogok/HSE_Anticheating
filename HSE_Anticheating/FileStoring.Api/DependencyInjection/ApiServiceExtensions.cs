using FileStoring.Api.Services;

namespace FileStoring.Api.DependencyInjection
{
    /// <summary>
    /// Класс, который регистрирует клиенты в DI-контейнере
    /// </summary>
    public static class ApiServiceExtensions
    {
        /// <summary>
        ///  Метод, который регистрирует клиенты в DI-контейнере
        /// </summary>
        /// <param name="services">Сервис, в который будут зарегистрированы клиенты</param>
        /// <returns>Тот же сервер, который был передан</returns>
        public static IServiceCollection AddFileStoringServices(
            this IServiceCollection services)
        {
            services.AddSingleton<IFileStorageService, FileStorageService>();
            return services;
        }
    }
}