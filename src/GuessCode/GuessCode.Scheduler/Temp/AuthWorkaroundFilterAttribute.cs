using Hangfire.Dashboard;

namespace GuessCode.Scheduler.Temp;

public class AuthWorkaroundFilterAttribute : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        return true;
    }
}