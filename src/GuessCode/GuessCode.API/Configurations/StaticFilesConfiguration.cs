using Microsoft.Extensions.FileProviders;

namespace GuessCode.API.Configurations;

public static class StaticFilesConfiguration
{
    public static void ApplyStaticFiles(this WebApplication app)
    {
        app.UseStaticFiles(new StaticFileOptions
        {
            RequestPath = "/static",
            FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
        });
    }
}