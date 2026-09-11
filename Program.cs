using VulnerableApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<JsonService>();
builder.Services.AddSingleton<DatabaseService>();
builder.Services.AddSingleton<SecurityService>();

var app = builder.Build();

app.MapControllers();

app.MapGet("/", () => "Vulnerable API - Auditoria de Seguridad");

app.Run();
