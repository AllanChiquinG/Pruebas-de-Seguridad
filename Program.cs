using VulnerableApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<JsonService>();

var app = builder.Build();

app.MapControllers();

app.MapGet("/", () => "Vulnerable API - Auditoria de Seguridad");

app.Run();
