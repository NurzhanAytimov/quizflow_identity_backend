using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using QuizIdentity.Application.DTOs.Account.Response;
using QuizIdentity.Application.Services.Interfaces;
using QuizIdentity.Domain.Entities.Identity;
using QuizIdentity.Infrastructure.Identity.Services.Interfaces;
using QuizIdentity.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using QuizIdentity.Domain.Settings;

namespace QuizIdentity.Infrastructure.Identity.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;

    private readonly IPasswordEncryptor _passwordEncryptor;

    private readonly ITokenService _tokenService;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public int _refteshExpiry;

    public AuthService(ApplicationDbContext context, IPasswordEncryptor passwordEncryptor, 
        ITokenService tokenService, IHttpContextAccessor httpContextAccessor,
        IOptions<JWTSettings> options)
    {
        _context = context;
        _passwordEncryptor = passwordEncryptor;
        _tokenService = tokenService;
        _httpContextAccessor = httpContextAccessor;
        _refteshExpiry = options.Value.RefreshExpiry;
    }

    public async Task<LoginResponseDto> Authenticate(string email, string password)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null) throw new KeyNotFoundException("Пользователь не найден!");

        var result = _passwordEncryptor.VerifyPassword(password, user.Password);
        if (!result) throw new KeyNotFoundException("Неправильный логин или пароль");

        var roles = user.UserRoles.Select(ur => ur.Role.Name).ToList();

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Email),
            new Claim("UserId", user.Id.ToString()),
        };

        foreach (var roleNames in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, roleNames));
        }

        var accessToken = _tokenService.GenerateAccessToken(claims);
        var refreshToken = _tokenService.GenerateRefreshToken();

        var userToken = await _context.UserTokens.FirstOrDefaultAsync(ut => ut.UserId == user.Id);

        if (userToken == null)
        {
            userToken = new UserToken
            {
                UserId = user.Id,
                RefreshToken = refreshToken,
                RefreshTokenExpiryTimeUtc = DateTime.UtcNow.AddMinutes(_refteshExpiry),
            };
            await _context.UserTokens.AddAsync(userToken);
        }
        else
        {
            userToken.RefreshToken = refreshToken;
            userToken.RefreshTokenExpiryTimeUtc = DateTime.UtcNow.AddMinutes(_refteshExpiry);
            _context.UserTokens.Update(userToken);
        }

        await _context.SaveChangesAsync();

        _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true, 
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddMinutes(_refteshExpiry)
        });

        return new LoginResponseDto
        {
            AccessToken = accessToken,
            Email = user.Email,
            IsEmailConfirmed = user.IsEmailConfirmed,
            Roles = roles
        };
    }

    public async Task<string> RefreshToken(string refreshToken)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(u => u.Role)
            .Include(u => u.UserToken)
            .FirstOrDefaultAsync(u => u.UserToken.RefreshToken == refreshToken);

        if (user == null || user.UserToken.RefreshTokenExpiryTimeUtc <= DateTimeOffset.UtcNow)
        {
            return null;
        }

        var roleNames = user.UserRoles.Select(ur => ur.Role.Name).ToList();

        var claims = new List<Claim>
        {
            new Claim("UserId", user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email)
        };
        foreach (var roleName in roleNames)
        {
            claims.Add(new Claim(ClaimTypes.Role, roleName));
        }

        var newAccessToken = _tokenService.GenerateAccessToken(claims);
        var newRefreshToken = _tokenService.GenerateRefreshToken();

        user.UserToken.RefreshToken = newRefreshToken;
        user.UserToken.RefreshTokenExpiryTimeUtc = DateTimeOffset.UtcNow.AddMinutes(_refteshExpiry);
        _context.UserTokens.Update(user.UserToken);
        await _context.SaveChangesAsync();

        var httpContext = _httpContextAccessor.HttpContext;

        httpContext.Response.Cookies.Append(
            "refreshToken",
            newRefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddMinutes(_refteshExpiry),
            }
        );

        return newAccessToken;
    }

    public async Task ConfirmEmailAsync(string email, string token)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null) throw new KeyNotFoundException("Пользователь не найден!");

        if (user.EmailConfirmationToken != token) throw new SecurityTokenException("Неверный токен подтверждения почты");

        user.IsEmailConfirmed = true;
        user.EmailConfirmationToken = null;
        await _context.SaveChangesAsync();
    }
}
