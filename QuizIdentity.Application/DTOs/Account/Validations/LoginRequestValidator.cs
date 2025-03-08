using FluentValidation;
using QuizIdentity.Application.DTOs.Account.Request;

namespace QuizIdentity.Application.DTOs.Account.Validations;

public sealed class LoginRequestValidator : AbstractValidator<LoginRequestDto>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
           .NotEmpty().NotNull().WithMessage("Вы не ввели Email")
           .EmailAddress().WithMessage("Введите корректный Email");

        RuleFor(x => x.Password)
            .NotEmpty().NotNull().WithMessage("Вы не ввели пароль")
            .MinimumLength(8).WithMessage("Минимальное кол-во символов 8");
    }
}
