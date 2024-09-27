using Hangfire;

var builder = WebApplication.CreateBuilder(args);

// Configuração do Hangfire usando o SQL Server
var hangfireConnectionString = builder.Configuration.GetConnectionString("HangfireConnection");
builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)  // Define o nível de compatibilidade dos dados.
    .UseSimpleAssemblyNameTypeSerializer()  // Utiliza um serializador simples de nomes de assemblies.
    .UseRecommendedSerializerSettings()  // Configurações recomendadas para o serializador.
    .UseSqlServerStorage(hangfireConnectionString));  // Armazenamento no SQL Server.
builder.Services.AddHangfireServer();

// Add services to the container.

var app = builder.Build();

// Configuração do painel de controle do Hangfire
app.UseHangfireDashboard();

// Configuração de endpoints do Hangfire Dashboard
app.UseRouting();

app.MapHangfireDashboard(); // Endpoint do painel de controle: /hangfire

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
});

EscreverConsole();
app.Run();

void EscreverConsole()
{
    var servico = new Service();
    RecurringJob.AddOrUpdate(() => servico.Data(), Cron.Minutely());

}
internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

public class Service
{
    public void Data()
    {
        Console.WriteLine(DateTime.Now.ToString());
    }
}

