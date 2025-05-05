using GuessCode.DAL.Models.BaseModels;

namespace GuessCode.DAL.Models.KataAggregate;

public class KataCodeExecutionResult : BaseEntity<Guid>
{
    public long ExecutedBy { get; set; }
    
    public long KataId { get; set; }
    
    public Kata Kata { get; set; }
    
    public string Output { get; set; }
    
    public string Error { get; set; }

    public decimal MemoryTaken { get; set; }
    
    public decimal TimeElapsed { get; set; }
    
    public int TotalTestCount { get; set; }
    
    public int PassedTestCount { get; set; }
}