using System.Text.Json.Serialization;
using GuessCode.DAL.Models.Enums;
using GuessCode.DAL.Models.KataAggregate;

namespace GuessCode.API.Models.V1.Kata;

public class KataDto
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
    
    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;
    
    [JsonPropertyName("programmingLanguage")]
    public ProgrammingLanguage ProgrammingLanguage { get; set; }
    
    [JsonPropertyName("kataDifficulty")]
    public KataDifficulty KataDifficulty { get; set; }
    
    [JsonPropertyName("kataType")]
    public KataType KataType { get; set; }

    [JsonPropertyName("kataJsonContent")]
    public KataJsonContent KataJsonContent { get; set; } = null!;

    [JsonPropertyName("authorId")]
    public long AuthorId { get; set; }
    
    [JsonPropertyName("memoryLimits")]
    public Dictionary<ProgrammingLanguage, decimal>? MemoryLimits { get; set; }
    
    [JsonPropertyName("timeLimits")]
    public Dictionary<ProgrammingLanguage, decimal>? TimeLimits { get; set; }
    
    [JsonPropertyName("isKataResolved")]
    public bool IsKataResolved { get; set; }
}