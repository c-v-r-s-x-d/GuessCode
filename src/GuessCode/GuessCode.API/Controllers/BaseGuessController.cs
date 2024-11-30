using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GuessCode.API.Controllers;

public class BaseGuessController : Controller
{
    protected long UserId { get; private set; } = -1;

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.HttpContext.Request.Headers.TryGetValue("UserId", out var userId))
        {
            UserId = Convert.ToInt64(userId.ToString());
        }
        
        await next();
    }
}