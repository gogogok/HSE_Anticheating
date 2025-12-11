using FileStoring.Api.DependencyInjection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

//Контроллеры + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//регистрируем все сервисы
builder.Services.AddFileStoringServices();

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

//Маршруты контроллеров
app.MapControllers();

app.Run();