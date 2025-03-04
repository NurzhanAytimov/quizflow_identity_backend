using QuizIdentity.Application.Services.Interfaces;

namespace QuizIdentity.Application.Services;

public class PasswordEncryptor : IPasswordEncryptor
{
    public string GeneratePassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string hassPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hassPassword);
    }
}
