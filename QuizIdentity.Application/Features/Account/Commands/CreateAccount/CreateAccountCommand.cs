using MediatR;
using QuizIdentity.Application.DTOs.Account.Request;
using QuizIdentity.Application.Wrapers;

namespace QuizIdentity.Application.Features.Account.Commands.CreateAccount;

public class CreateAccountCommand(CreateAccountRequestDto dto) : IRequest<Response<bool>>
{
    public CreateAccountRequestDto Dto { get; set; } = dto;
}
