namespace QuizIdentity.Domain.Entities.Identity;

public class UserToken 
{
    public long Id { get; set; }

    public string? RefreshToken { get; set; }

    public DateTimeOffset RefreshTokenExpiryTimeUtc { get; set; }

    public long UserId { get; set; }

    #region Navigation properties

    public User? User { get; set; }

    #endregion
}
