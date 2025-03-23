using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuizIdentity.Application.DTOs.Account.Request;
using QuizIdentity.Application.Features.Account.Commands.CreateAccount;
using QuizIdentity.Application.Wrapers;
using QuizIdentity.Infrastructure.Identity.Services.Interfaces;

namespace QuizIdenitity.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly IAuthService _authService;

    private readonly IPasswordResetService _passwordResetService;

    public AccountController(IMediator mediator, IAuthService authService, IPasswordResetService passwordResetService)
    {
        _mediator = mediator;
        _authService = authService;
        _passwordResetService = passwordResetService;
    }

    /// <summary>
    /// Регистрация пользователя
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("registration")]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequestDto command)
    {
        return Ok(await _mediator.Send(new CreateAccountCommand(command)));
    }

    /// <summary>
    /// Авторизация
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Token([FromBody] LoginRequestDto dto)
    {
        return Ok(await _authService.Authenticate(dto.Email, dto.Password));
    }

    [HttpPost("refresh")]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]

    public async Task<ActionResult<string>> RefreshToken()
    {
        if (!HttpContext.Request.Cookies.TryGetValue("refreshToken", out var refreshToken) || string.IsNullOrWhiteSpace(refreshToken))
        {
            return BadRequest("Токен обновления отсутствует или недействителен в файлах cookie!");
        }

        var response = await _authService.RefreshToken(refreshToken);
        if (response == null)
        {
            return Unauthorized("Не валидный токен!");
        }

        return Ok(response);
    }
    /// <summary>
    /// Подтверждение почты
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpGet("confirm-email")]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]

    public async Task<IActionResult> ConfirmEmail(string email, string token)
    {
        await _authService.ConfirmEmailAsync(email, token);
        return Ok("Почта успешно подтверждена");
    }

    [HttpPost("forgot-password")]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto dto)
    {
        await _passwordResetService.RequestPasswordResetAsync(dto.Email);
        return Ok("Письмо для сброса пароля отправлено");
    }

    [HttpPost("reset-password")]
    [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDto dto)
    {
        await _passwordResetService.ResetPasswordAsync(dto.Email, dto.Token, dto.NewPassword);
        return Ok("Пароль успешно изменен");
    }
}
