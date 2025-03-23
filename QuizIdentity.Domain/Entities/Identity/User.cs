using QuizIdentity.Domain.Entities.Subscriptions;

namespace QuizIdentity.Domain.Entities.Identity;

public class User 
{
    public long Id { get; set; }

    public string? UserName { get; set; }

    public required string Email { get; set; }

    public bool IsDelete { get; set; } = false;

    public required string Password { get; set; }

    public DateTimeOffset CreateDateUtch { get; set; }

    public string? EmailConfirmationToken { get; set; }

    public bool IsEmailConfirmed { get; set; } = false;

    public string? PasswordResetToken { get; set; }

    public DateTimeOffset? PasswordResetTokenExpiry { get; set; }

    #region Navigation properties

    public ICollection<UserRole>? UserRoles { get; set; }

    public UserToken? UserToken { get; set; }

    public Subscription? Subscription { get; set; }

    #endregion
}
