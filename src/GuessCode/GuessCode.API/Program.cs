using GuessCode.API.Configurations;
using GuessCode.Domain.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.AddPrimaryConfiguration();
builder.AddBusinessLogicConfiguration();
builder.AddCorsConfiguration();
builder.AddAuthConfiguration();
builder.AddSettingsConfiguration();
builder.AddDbConfiguration();
builder.AddRedisConfiguration();
builder.AddOpenTelemetryConfiguration();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAll");
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHub<UserStatusHub>("/status-hub");
});

app.ApplyDatabaseMigrations();
app.Run();
