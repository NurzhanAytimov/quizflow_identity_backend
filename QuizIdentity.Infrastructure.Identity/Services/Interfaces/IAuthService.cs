using QuizIdentity.Application.DTOs.Account.Response;

namespace QuizIdentity.Infrastructure.Identity.Services.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDto> Authenticate(string username, string password);
}
