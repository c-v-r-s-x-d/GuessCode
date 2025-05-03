namespace GuessCode.API.Models.V1.Kata;

public class KataBugFindingSolveResultDto
{
    public bool IsAnswerCorrect { get; set; }
    
    public string? Error { get; set; }
    
    public int? PointsEarned { get; set; }
}