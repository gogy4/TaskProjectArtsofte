using Application.Models;
using FluentValidation;
using Shared.Services;

namespace Application.Validators;

public class CreateNotificationDtoValidator : AbstractValidator<CreateNotificationDto>
{
    private readonly HttpClient httpClient;

    public CreateNotificationDtoValidator(HttpClient httpClient)
    {
        this.httpClient = httpClient;

        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required");
        RuleFor(_ => true)
            .MustAsync(async (_, cancellationToken) => await IsUserLogin())
            .WithMessage("Пользователь не авторизован.");
        RuleFor(x => x.UserId)
            .MustAsync(async (userId, cancellationToken) => await IsReceiverExists(userId))
            .WithMessage("Пользователя не существует.");
    }

    private async Task<bool> IsUserLogin()
    {
        var user = await UserHelper.GetCurrentUserId(httpClient);
        return true;
    }

    private async Task<bool> IsReceiverExists(int userId)
    {
        return await UserHelper.IsReceiverExists(httpClient, userId);
    }
}