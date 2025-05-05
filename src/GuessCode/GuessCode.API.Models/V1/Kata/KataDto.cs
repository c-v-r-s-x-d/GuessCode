using GuessCode.DAL.Models.Enums;
using GuessCode.DAL.Models.KataAggregate;

namespace GuessCode.API.Models.V1.Kata;

public class KataDto
{
    public long Id { get; set; }
    
    public string Title { get; set; } = null!;
    
    public ProgrammingLanguage ProgrammingLanguage { get; set; }
    
    public KataDifficulty KataDifficulty { get; set; }
    
    public KataType KataType { get; set; }

    public KataJsonContent KataJsonContent { get; set; } = null!;

    public long AuthorId { get; set; }
    
    public Dictionary<ProgrammingLanguage, decimal>? MemoryLimits { get; set; }
    
    public Dictionary<ProgrammingLanguage, decimal>? TimeLimits { get; set; }
    
    public bool IsKataResolved { get; set; }
}