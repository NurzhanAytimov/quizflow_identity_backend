namespace QuizIdentity.Infrastructure.Shared.Services.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string emailFrom, string body, string subject);

    Task SendEmailConfirmationAsync(string email, string confirmationLink);
}
