using GuessCode.DAL.Models.Enums;
using GuessCode.DAL.Models.KataAggregate;

namespace GuessCode.API.Models.V1.Kata;

public class KataDto
{
    public string Title { get; set; } = null!;
    
    public ProgrammingLanguage ProgrammingLanguage { get; set; }
    
    public KataDifficulty KataDifficulty { get; set; }
    
    public KataType KataType { get; set; }
    
    public KataJsonContent? KataRawJsonContent { get; set; }

    public long AuthorId { get; set; }
}