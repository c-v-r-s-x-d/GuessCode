namespace GuessCode.Domain.Models;

public class KataSolveResult
{
    public bool IsAnswerCorrect { get; set; }
    
    public string? Error { get; set; }
}