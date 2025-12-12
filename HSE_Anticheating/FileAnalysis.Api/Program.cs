using FileAnalysis.Api.DependencyInjection;
using FileAnalysis.Core.Domain.Services;
using FileAnalysis.Infrastructure.DependencyInjection;
using FileAnalysis.Infrastructure.Persistence;
using FileAnalysis.Infrastructure.Services;
using Microsoft.Extensions.FileProviders;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

//Контроллеры + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Регистрация инфраструктуры
builder.Services.AddInfrastructure(builder.Configuration);

//Регистрация бизнес-сервисов
builder.Services.AddApiServices();

WebApplication app = builder.Build();

//создаём таблицы
using (IServiceScope scope = app.Services.CreateScope())
{
    FileAnalysisDbContext db = scope.ServiceProvider.GetRequiredService<FileAnalysisDbContext>();
    db.Database.EnsureCreated();
}

string wordCloudFolder = Path.Combine(AppContext.BaseDirectory, "WordClouds");
Directory.CreateDirectory(wordCloudFolder);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(wordCloudFolder),
    RequestPath = "/wordclouds"
});


app.UseSwagger();
app.UseSwaggerUI();

//Маршруты контроллеров
app.MapControllers();

app.Run();