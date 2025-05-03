namespace GuessCode.Domain.Models;

public class KataCodeReadingSolveResult
{
    public bool IsAnswerCorrect { get; set; }
    
    public string? Error { get; set; }
    
    public int? PointsEarned { get; set; }
}