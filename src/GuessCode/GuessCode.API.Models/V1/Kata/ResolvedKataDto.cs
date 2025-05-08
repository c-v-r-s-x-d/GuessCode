namespace GuessCode.API.Models.V1.Kata;

public class ResolvedKataDto
{
    public long KataId { get; set; }
    
    public long ResolvedUserId { get; set; }
    
    public long PointEarned { get; set; }
    
    public string? SourceCode { get; set; }
    
    public int? SelectedOptionId { get; set; }
    
    public string? ExecutionOutput { get; set; }
}