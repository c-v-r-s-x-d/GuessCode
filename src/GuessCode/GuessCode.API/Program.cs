using GuessCode.API.Configurations;
using GuessCode.Domain.Hubs;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.AddPrimaryConfiguration();
builder.AddBusinessLogicConfiguration();
builder.AddCorsConfiguration();
builder.AddAuthConfiguration();
builder.AddSettingsConfiguration();
builder.AddDbConfiguration();
builder.AddRedisConfiguration();
builder.AddOpenTelemetryConfiguration();
builder.AddLoggingConfiguration();
    
builder.Host.UseSerilog();


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseExceptionHandler();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("GuessCodeCorsPolicy");
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<UserStatusHub>("/status-hub");
});

app.ApplyDatabaseMigrations();
app.Run();
