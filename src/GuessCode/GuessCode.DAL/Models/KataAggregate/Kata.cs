using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;
using GuessCode.DAL.Models.BaseModels;
using GuessCode.DAL.Models.Enums;
using GuessCode.DAL.Models.UserAggregate;

namespace GuessCode.DAL.Models.KataAggregate;

public class Kata : BaseEntity<long>
{
    public string Title { get; set; }

    public ProgrammingLanguage ProgrammingLanguage { get; set; }

    public KataDifficulty KataDifficulty { get; set; }

    public KataType KataType { get; set; }
    
    public List<KataCodeExecutionResult> KataCodeExecutionResults { get; set; }
    
    public int PointsReward { get; set; }
    
    public Dictionary<ProgrammingLanguage, decimal> MemoryLimits { get; set; }
    
    public Dictionary<ProgrammingLanguage, decimal> TimeLimits { get; set; }
    
    public bool IsApproved { get; set; }

    [JsonIgnore]
    public string KataRawJsonContent { get; set; }

    [NotMapped]
    public KataJsonContent KataJsonContent
    {
        get => string.IsNullOrEmpty(KataRawJsonContent)
            ? new KataJsonContent()
            : JsonSerializer.Deserialize<KataJsonContent>(KataRawJsonContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new KataJsonContent();

        set => KataRawJsonContent = JsonSerializer.Serialize(value);
    }

    public long AuthorId { get; set; }
    public User Author { get; set; }
}

public class KataJsonContent
{
    [JsonPropertyName("kataDescription")]
    public string KataDescription { get; set; }

    [JsonPropertyName("sourceCode")]
    public string SourceCode { get; set; }

    [JsonPropertyName("answerOptions")]
    public List<AnswerOption> AnswerOptions { get; set; }

    public List<AnswerOption> GetPublicAnswerOptions()
    {
        return AnswerOptions.Select(opt => new AnswerOption
        {
            OptionId = opt.OptionId,
            Option = opt.Option
        }).ToList();
    }
}

public class AnswerOption
{
    [JsonPropertyName("optionId")]
    public int OptionId { get; set; }

    [JsonPropertyName("option")]
    public string Option { get; set; }

    [JsonPropertyName("isCorrect")]
    public bool IsCorrect { get; set; }
}
