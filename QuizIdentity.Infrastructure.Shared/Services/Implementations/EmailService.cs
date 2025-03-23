using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using QuizIdentity.Infrastructure.Shared.Services.Interfaces;

namespace QuizIdentity.Infrastructure.Shared.Services.Implementations;

public class EmailService : IEmailService
{
    private readonly string _smtpEmail;

    private readonly string _password;

    private readonly string _smtpServer;

    private readonly int _port;

    private readonly string _dispayName;

    private readonly ILogger<EmailService> _logger;

    //private readonly string _emailFrom;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        var emailConfig = configuration.GetSection("EmailSettings");
        _smtpEmail = emailConfig["SmtpUser"];
        _password = emailConfig["SmtpPass"];
        _smtpServer = emailConfig["SmtpHost"];
        _port = int.Parse(emailConfig["SmtpPort"]);
        _dispayName = emailConfig["DisplayName"];
        _logger = logger;
    }

    public async Task SendEmailAsync(string emailFrom, string body, string subject)
    {
        try
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_dispayName, _smtpEmail));
            emailMessage.To.Add(MailboxAddress.Parse(emailFrom));
            emailMessage.Subject = subject;
            emailMessage.Body = new BodyBuilder
            {
                HtmlBody = body
            }.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_smtpServer, _port, SecureSocketOptions.Auto);
            smtp.Authenticate(_smtpEmail, _password);
            await smtp.SendAsync(emailMessage);
            smtp.Disconnect(true);

        }
        catch (SmtpCommandException ex)
        {
            _logger.LogError(ex, "Ошибка SMTP при отправке Email: завершилась с ошибкой: {Message}", ex.Message);
        }
        catch (SmtpProtocolException ex)
        {
            _logger.LogError(ex, "Ошибка протокола SMTP при отправке Email: {Message}", ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Общая ошибка при отправке Email: {Message}", ex.Message);
        }
    }

    public async Task SendEmailConfirmationAsync(string email, string confirmationLink)
    {
        var subject = "Подтверждение почты";
        var body = $"Пожалуйста, подтвердите вашу почту, перейдя по <a href='{confirmationLink}'>ссылке</a>.";
        await SendEmailAsync(email, body, subject);
    }
}
