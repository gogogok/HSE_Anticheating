using FileAnalysis.Core.Application.Interfaces;
using FileAnalysis.Core.Application.Services;

namespace FileAnalysis.Api.DependencyInjection
{
    /// <summary>
    /// Класс, который регистрирует клиенты в DI-контейнере
    /// </summary>
    public static class ApiServiceExtensions
    {
        /// <summary>
        /// Метод, который регистрирует клиенты в DI-контейнере
        /// </summary>
        /// <param name="services">Сервис, в который будут зарегистрированы клиенты</param>
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddScoped<IWorkService, WorkService>();
            services.AddScoped<IReportService, ReportService>();

            return services;
        }
    }
}