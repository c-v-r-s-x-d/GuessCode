using GuessCode.Scheduler.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.AddRedisConfiguration();
builder.AddDatabaseConfiguration();
builder.AddMediatrConfiguration();
builder.AddHangfireConfiguration();
builder.AddSettingsConfiguration();
builder.AddBusinessLogicConfiguration();

var app = builder.Build();

app.ConfigureHangfire();

app.Run();