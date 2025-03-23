namespace QuizIdentity.Infrastructure.Identity.Services.Interfaces;

public interface IPasswordResetService
{
    Task RequestPasswordResetAsync(string email);
    Task<bool> VerifyPasswordResetTokenAsync(string email, string token);
    Task ResetPasswordAsync(string email, string token, string newPassword);
}
