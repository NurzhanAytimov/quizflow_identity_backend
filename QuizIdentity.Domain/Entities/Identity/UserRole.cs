using Microsoft.AspNetCore.Identity;

namespace QuizIdentity.Domain.Entities.Identity;

public class UserRole
{
    public long UserId { get; set; }

    public int RoleId { get; set; }

    #region Navigation properties

    public User? User { get; set; }

    public Role? Role { get; set; }
    #endregion
}
