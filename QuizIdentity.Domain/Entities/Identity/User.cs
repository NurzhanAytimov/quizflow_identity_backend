using Microsoft.AspNetCore.Identity;

namespace QuizIdentity.Domain.Entities.Identity;

public class User 
{
    public long Id { get; set; }

    public string? UserName { get; set; }

    public required string Email { get; set; }

    public bool IsDelete { get; set; } = false;

    public required string Password { get; set; }

    #region Navigation properties

    public ICollection<UserRole>? UserRoles { get; set; }

    public UserToken? UserToken { get; set; }

    #endregion
}
