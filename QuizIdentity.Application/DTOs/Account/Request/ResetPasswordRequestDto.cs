namespace QuizIdentity.Application.DTOs.Account.Request;

public class ResetPasswordRequestDto
{
    public required string Email { get; set; }

    public required string Token { get; set; }

    public required string NewPassword { get; set; }
}
