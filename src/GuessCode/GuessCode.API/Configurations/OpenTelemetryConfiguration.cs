using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace GuessCode.API.Configurations;

public static class OpenTelemetryConfiguration
{
    public static void AddOpenTelemetryConfiguration(this WebApplicationBuilder builder)
    {
        var otelUri = builder.Configuration["OpenTelemetry:Host"]!;
        
        builder.WebHost.UseSentry(options =>
        {
            options.Dsn = "";
            options.Debug = true;
            options.TracesSampleRate = 1.0;
        });

        builder.Logging.ClearProviders();
        builder.Logging.AddOpenTelemetry(options =>
        {
            options.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("GuessCode"));
            options.AddOtlpExporter(o => o.Endpoint = new Uri(otelUri));
        });

        builder.Services.AddOpenTelemetry()
            .WithTracing(tracerProviderBuilder => tracerProviderBuilder
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("GuessCode"))
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddOtlpExporter(o => o.Endpoint = new Uri(otelUri)))
            .WithMetrics(metricsProviderBuilder => metricsProviderBuilder
                .AddAspNetCoreInstrumentation()
                .AddRuntimeInstrumentation()
                .AddPrometheusExporter());
    }
}