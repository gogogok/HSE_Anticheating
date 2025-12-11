using FileAnalysis.Core.Application.Interfaces;
using FileAnalysis.Core.Domain.Services;
using FileAnalysis.Infrastructure.FileAccess;
using FileAnalysis.Infrastructure.Persistence;
using FileAnalysis.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FileAnalysis.Infrastructure.DependencyInjection
{
    /// <summary>
    /// Класс, который регистрирует клиенты в DI-контейнере
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            //добавляем что-то типо базы данных
            services.AddSingleton(new JsonDatabase("AppData"));

            services.AddSingleton<IWorkRepository, WorkRepository>();
            services.AddSingleton<IReportRepository, ReportRepository>();

            services.AddSingleton<IPlagiarismDetector, PlagiarismDetector>();
            services.AddSingleton<IWordCloudGenerator, WordCloudGenerator>();

            //регистрация клиента хранилища
            services.AddHttpClient<IFileContentProvider, FileContentProvider>(client =>
            {
                client.BaseAddress = new Uri(configuration["FileStoringApi:Url"] ?? string.Empty);
            });

            return services;
        }
    }
}