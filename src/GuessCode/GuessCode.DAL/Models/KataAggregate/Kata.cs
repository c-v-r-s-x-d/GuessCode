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
    
    public string KataRawJsonContent { get; set; }
    
    [NotMapped]
    public KataJsonContent KataJsonContent => string.IsNullOrEmpty(KataRawJsonContent)
        ? new KataJsonContent()
        : JsonConvert.DeserializeObject<KataJsonContent>(KataRawJsonContent);
    
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
    public string AnswerOptionsRawJson { get; set; }
    
    [NotMapped]
    [JsonIgnore]
    public List<AnswerOption> AnswerOptions => string.IsNullOrEmpty(AnswerOptionsRawJson)
        ? new List<AnswerOption>()
        : JsonConvert.DeserializeObject<List<AnswerOption>>(AnswerOptionsRawJson);
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
