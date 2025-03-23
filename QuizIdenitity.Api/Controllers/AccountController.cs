using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuizIdentity.Application.DTOs.Account.Request;
using QuizIdentity.Application.Features.Account.Commands.CreateAccount;
using QuizIdentity.Infrastructure.Identity.Services.Interfaces;

namespace QuizIdenitity.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;

    private readonly IAuthService _authService;

    public AccountController(IMediator mediator, IAuthService authService)
    {
        _mediator = mediator;
        _authService = authService;
    }

    /// <summary>
    /// Регистрация пользователя
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("registration")]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountRequestDto command)
    {
        return Ok(await _mediator.Send(new CreateAccountCommand(command)));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Token([FromBody] LoginRequestDto dto)
    {
        return Ok(await _authService.Authenticate(dto.Email, dto.Password));
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail(string email, string token)
    {
        await _authService.ConfirmEmailAsync(email, token);
        return Ok("Почта успешно подтверждена");
    }
}
