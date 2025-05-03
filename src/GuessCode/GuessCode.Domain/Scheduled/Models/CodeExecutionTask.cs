namespace GuessCode.Domain.Scheduled.Models;

public class CodeExecutionTask
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    public long ExecutedBy { get; set; }
    
    public long KataId { get; set; }
    
    public string Language { get; set; }
    
    public string SourceCode { get; set; }
    
    public string Input { get; set; }
}