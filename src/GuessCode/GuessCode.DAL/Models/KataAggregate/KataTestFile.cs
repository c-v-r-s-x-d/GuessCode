namespace GuessCode.DAL.Models.KataAggregate;

public class KataTestFile
{
    public long KataId { get; set; }
    
    public Kata Kata { get; set; }
    public Guid FileId { get; set; }
}