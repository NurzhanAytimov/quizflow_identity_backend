namespace QuizIdentity.Application.Services.Interfaces;

public interface IPasswordEncryptor
{
    string GeneratePassword(string password);

    bool VerifyPassword(string password, string hassPassword);
}
