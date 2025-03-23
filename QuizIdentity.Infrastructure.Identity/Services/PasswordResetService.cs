using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuizIdentity.Application.Services.Interfaces;
using QuizIdentity.Infrastructure.Identity.Services.Interfaces;
using QuizIdentity.Infrastructure.Persistence.Context;
using QuizIdentity.Infrastructure.Shared.Services.Interfaces;

namespace QuizIdentity.Infrastructure.Identity.Services;

public class PasswordResetService : IPasswordResetService
{
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;
    private readonly ITokenService _tokenService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IPasswordEncryptor _passwordEncryptor;
    private readonly string _baseUrl;

    public PasswordResetService(
      ApplicationDbContext context,
      IEmailService emailService,
      ITokenService tokenService,
      IHttpContextAccessor httpContextAccessor,
      IPasswordEncryptor passwordEncryptor, IConfiguration configuration)
    {
        _context = context;
        _emailService = emailService;
        _tokenService = tokenService;
        _httpContextAccessor = httpContextAccessor;
        _passwordEncryptor = passwordEncryptor;
        _baseUrl = configuration[$"EmailSettings:BaseUrl"];
    }

    public async Task RequestPasswordResetAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null) throw new KeyNotFoundException("Пользователь не найден");

        var token = _tokenService.GeneratePasswordResetToken();
        user.PasswordResetToken = token;
        user.PasswordResetTokenExpiry = DateTimeOffset.UtcNow.AddMinutes(10); 

        await _context.SaveChangesAsync();

        await _emailService.SendPasswordResetAsync(user.Email, _baseUrl);
    }

    public async Task<bool> VerifyPasswordResetTokenAsync(string email, string token)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null) return false;

        return user.PasswordResetToken == token && user.PasswordResetTokenExpiry > DateTimeOffset.UtcNow;
    }

    public async Task ResetPasswordAsync(string email, string token, string newPassword)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null) throw new KeyNotFoundException("Пользователь не найден");

        if (user.PasswordResetToken != token || user.PasswordResetTokenExpiry < DateTimeOffset.UtcNow)
            throw new SecurityTokenException("Неверный или просроченный токен сброса пароля");

        user.Password = _passwordEncryptor.GeneratePassword(newPassword);
        user.PasswordResetToken = null;
        user.PasswordResetTokenExpiry = null;

        await _context.SaveChangesAsync();
    }
}
