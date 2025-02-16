using GuessCode.DAL.External.Contracts;
using GuessCode.DAL.External.Services;
using GuessCode.Domain.Auth.Contracts;
using GuessCode.Domain.Auth.Services;
using GuessCode.Domain.Contracts;
using GuessCode.Domain.File.Contracts;
using GuessCode.Domain.File.Services;
using GuessCode.Domain.Services;

namespace GuessCode.API.Configurations;

public static class BusinessLogicConfiguration
{
    public static void AddBusinessLogicConfiguration(this IHostApplicationBuilder builder)
    {
        builder.Services.AddScoped<IUserLoginService, UserLoginService>();
        builder.Services.AddScoped<IUserRegisterService, UserRegisterService>();
        builder.Services.AddScoped<IKataSearchService, KataSearchService>();
        builder.Services.AddScoped<IKataSolveService, KataSolveService>();
        builder.Services.AddScoped<IKataAdministrationService, KataAdministrationService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IProfileInfoService, ProfileInfoService>();
        builder.Services.AddScoped<ILeaderboardService, LeaderboardService>();
        builder.Services.AddScoped<IUserStatusUpdateService, UserStatusUpdateService>();
        builder.Services.AddScoped<IGitHubLoginService, GitHubLoginService>();
        builder.Services.AddScoped<IFileUploaderService, FileUploaderService>();
        builder.Services.AddScoped<IHttpService, HttpService>();
    }
}