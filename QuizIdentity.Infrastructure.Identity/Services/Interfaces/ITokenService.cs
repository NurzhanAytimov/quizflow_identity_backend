using System.Security.Claims;

namespace QuizIdentity.Infrastructure.Identity.Services.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);

    string GenerateRefreshToken();

    ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken);

    string GeneratePasswordResetToken();
}
