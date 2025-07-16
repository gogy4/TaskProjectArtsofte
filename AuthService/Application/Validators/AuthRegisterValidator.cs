using Application.Models;
using FluentValidation;
using Infrastructure.Repository.Abstractions;

namespace Application.Validators;

public class AuthRegisterValidator : AbstractValidator<AuthRequest>
{
    private readonly IUserRepository repository;
    
    public AuthRegisterValidator(IUserRepository userRepository)
    {
        repository = userRepository;
        
        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("Email не является действительным")
            .MustAsync(async (email, token) => await IsEmailNotExists(email))
            .WithMessage("Данная почта уже зарегистрирована");
        
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Пароль не должен быть пустым")
            .MinimumLength(8).WithMessage("Пароль должен быть не менее 8 символов")
            .MaximumLength(25).WithMessage("Пароль должен быть не более 25 символов")
            .Matches(@"[A-Z]+").WithMessage("Пароль должен содержать хотя бы одну заглавную букву")
            .Matches(@"[a-z]+").WithMessage("Пароль должен содержать хотя бы одну строчную букву")
            .Matches(@"[!@#$%^&*()]+").WithMessage("Пароль должен содержать хотя бы один специальный символ: !@#$%^&*()");
    }

    private async Task<bool> IsEmailNotExists(string email)
    {
        var user = await repository.GetByEmailAsync(email);
        return user is null;
    }
}