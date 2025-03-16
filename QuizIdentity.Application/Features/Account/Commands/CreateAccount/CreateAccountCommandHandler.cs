using MediatR;
using QuizIdentity.Application.DTOs.Account.Response;
using QuizIdentity.Application.Services.Interfaces;
using QuizIdentity.Application.Wrapers;
using QuizIdentity.Domain.Entities.Identity;
using QuizIdentity.Infrastructure.Persistence.Context;

namespace QuizIdentity.Application.Features.Account.Commands.CreateAccount;

internal sealed class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Response<CreateLoginResponseDto>>
{
    private readonly ApplicationDbContext _context;

    private readonly IPasswordEncryptor _passwordEncryptor;

    public CreateAccountCommandHandler(ApplicationDbContext context, IPasswordEncryptor passwordEncryptor)
    {
        _context = context;
        _passwordEncryptor = passwordEncryptor;
    }

    public async Task<Response<CreateLoginResponseDto>> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        string password = _passwordEncryptor.GeneratePassword(request.Dto.Password);

        var user = new User
        {
            Email = request.Dto.Email,
            Password = password,
            IsDelete = false,
            CreateDateUtch = DateTimeOffset.UtcNow
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync(cancellationToken);

        var result = new CreateLoginResponseDto(user);
        return new Response<CreateLoginResponseDto>(result, "Аккаунт успешно создан");
    }
}
