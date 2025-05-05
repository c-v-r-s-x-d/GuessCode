using System.ComponentModel.DataAnnotations.Schema;
using GuessCode.DAL.Models.BaseModels;
using GuessCode.DAL.Models.Enums;
using GuessCode.DAL.Models.UserAggregate;
using Newtonsoft.Json;

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

    [JsonIgnore]
    public string KataRawJsonContent { get; set; }

    [NotMapped]
    public KataJsonContent KataJsonContent
    {
        get => string.IsNullOrEmpty(KataRawJsonContent)
            ? new KataJsonContent()
            : JsonConvert.DeserializeObject<KataJsonContent>(KataRawJsonContent) ?? new KataJsonContent();

        set => KataRawJsonContent = JsonConvert.SerializeObject(value);
    }

    public long AuthorId { get; set; }
    public User Author { get; set; }
}

public class KataJsonContent
{
    [JsonProperty("kata_description")]
    public string KataDescription { get; set; }

    [JsonProperty("source_code")]
    public string SourceCode { get; set; }

    [JsonProperty("answer_options")]
    public List<AnswerOption> AnswerOptions { get; set; } = new();

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
    [JsonProperty("option_id")]
    public int OptionId { get; set; }

    [JsonProperty("option")]
    public string Option { get; set; }

    [JsonProperty("is_correct")]
    public bool IsCorrect { get; set; }
}
