namespace QuizIdentity.Application.DTOs.Account.Response;

public class LoginResponseDto
{
    public string? AccessToken { get; set; }

    public string? Email { get; set; }

    public List<string>? Roles { get; set; } = null;
}
