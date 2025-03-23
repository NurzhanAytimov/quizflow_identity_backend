using FluentValidation;
using QuizIdentity.Application.DTOs.Account.Request;

namespace QuizIdentity.Application.Features.Account.Commands.Validation;

internal sealed class CreateAccountValidation : AbstractValidator<CreateAccountRequestDto>
{
    public CreateAccountValidation()
    {
        RuleFor(x => x.Email)
           .NotEmpty().NotNull().WithMessage("Вы не ввели Email")
           .EmailAddress().WithMessage("Введите корректный Email");

        RuleFor(x => x.Password)
            .NotEmpty().NotNull().WithMessage("Вы не ввели пароль")
            .MinimumLength(8).WithMessage("Минимальное кол-во символов 8");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().NotNull().WithMessage("Вы не ввели Подтверждение пароля")
            .MinimumLength(8).WithMessage("Минимальное кол-во символов 8");

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage("Вы не ввели Подтверждение пароля")
            .MinimumLength(8).WithMessage("Минимальное кол-во символов 8")
            .Equal(x => x.Password).WithMessage("Пароли не совпадают");
    }
}
