using FileAnalysis.Core.Application.Interfaces;
using FileAnalysis.Core.Domain.Services;
using FileAnalysis.Infrastructure.FileAccess;
using FileAnalysis.Infrastructure.Persistence;
using FileAnalysis.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

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
            services.AddDbContext<FileAnalysisDbContext>(options =>
                options.UseSqlite("Data Source=data/fileanalysis.db"));

            services.AddScoped<IWorkRepository, EfWorkRepository>();
            services.AddScoped<IReportRepository, EfReportRepository>();

            services.AddScoped<IPlagiarismDetector, PlagiarismDetector>();
            services.AddScoped<IWordCloudGenerator, WordCloudGenerator>();

            //регистрация клиента хранилища
            services.AddHttpClient<IFileContentProvider, FileContentProvider>(client =>
            {
                client.BaseAddress = new Uri(configuration["FileStoringApi:Url"] ?? string.Empty);
            });

            return services;
        }
    }
}