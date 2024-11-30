namespace GuessCode.API.Models.V1.Kata;

public class KataSolveResultDto
{
    public bool IsAnswerCorrect { get; set; }
    
    public string? Error { get; set; }
}