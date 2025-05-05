namespace GuessCode.DAL.Commands;

public class BaseEmail
{
    public Guid RequestId { get; } = Guid.NewGuid();
    
    public string ReceiverEmail { get; set; }
    
    public string Username { get; set; }
}