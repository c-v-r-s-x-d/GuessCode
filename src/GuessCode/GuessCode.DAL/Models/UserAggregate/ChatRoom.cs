namespace GuessCode.DAL.Models.UserAggregate;

public class ChatRoom
{
    public long User1Id { get; set; }
    
    public long User2Id { get; set; }
    
    public Guid RoomId { get; set; }
}