namespace QuizIdentity.Domain.Entities.Identity;

public class Role
{
    public int Id { get; set; }

    public required string Name { get; set; }

    #region Navigation properties

    public ICollection<UserRole>? UserRoles { get; set; }

    #endregion
}
