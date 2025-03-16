using QuizIdentity.Domain.Entities.Identity;

namespace QuizIdentity.Application.DTOs.Account.Response;

public class CreateLoginResponseDto(User user)
{
    public string? Email { get; set; } = user.Email;

    public DateTimeOffset? CreateDate { get; set; } = user.CreateDateUtch;
}
