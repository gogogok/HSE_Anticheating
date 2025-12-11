using Gateway.Api.DependencyInjection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

//Контроллеры + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//HTTP-клиенты к FileStoring и FileAnalysis, регистрация сервисов
builder.Services.AddGatewayServices(builder.Configuration);

WebApplication app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

//Маршруты контроллеров
app.MapControllers();

app.Run();