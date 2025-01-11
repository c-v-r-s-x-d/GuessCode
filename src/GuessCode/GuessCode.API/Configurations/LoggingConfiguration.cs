using Serilog;
using Serilog.Sinks.Grafana.Loki;

namespace GuessCode.API.Configurations;

public static class LoggingConfiguration
{
    public static void AddLoggingConfiguration(this WebApplicationBuilder builder)
    {
        var lokiUrl = builder.Configuration["Logging:Loki:Host"]!;
        
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.GrafanaLoki(lokiUrl)
            .CreateLogger();
    }
}