using System.Net;

namespace GuessCode.API.Models;

public class DefaultExceptionMessage
{
    public HttpStatusCode Status { get; set; }
    
    public string ErrorMessage { get; set; } = null!;

    public string? ErrorSource { get; set; }
}