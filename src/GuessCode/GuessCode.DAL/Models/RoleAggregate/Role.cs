using GuessCode.DAL.Models.BaseModels;
using GuessCode.DAL.Models.UserAggregate;

namespace GuessCode.DAL.Models.RoleAggregate;

public class Role : BaseEntity<long>
{
    public string Name { get; set; }
    
    public List<User> Users { get; set; }
}