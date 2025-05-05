namespace GuessCode.API.Models.V1.Settings;

public class AISettings
{
    public string ApiKey { get; set; } = null!;
    
    public string Model { get; set; } = null!;
}