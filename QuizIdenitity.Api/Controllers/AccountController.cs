using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuizIdentity.Application.DTOs.Account.Request;
using QuizIdentity.Application.Features.Account.Commands.CreateAccount;

namespace QuizIdenitity.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
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
}
