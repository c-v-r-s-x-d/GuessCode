namespace GuessCode.DAL.Models.KataAggregate;

public class KataTestFile
{
    public long KataId { get; set; }
    
    public Kata Kata { get; set; }
    public string FileName { get; set; }
}