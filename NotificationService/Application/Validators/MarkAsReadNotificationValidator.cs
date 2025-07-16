using Application.Models;
using FluentValidation;
using Shared.Services;

namespace Application.Validators;

public class MarkAsReadNotificationValidator : AbstractValidator<NotificationDto>
{
    private readonly HttpClient httpClient;

    public MarkAsReadNotificationValidator(HttpClient httpClient)
    {
        this.httpClient = httpClient;
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required");
        RuleFor(x => x.UserId)
            .MustAsync(async (userId, cancellationToken) => await IsUserLogin(userId))
            .WithMessage("Пользователь не авторизован или это сообщение принадлежит другому пользователю");
    }

    private async Task<bool> IsUserLogin(int userId)
    {
        var user = await UserHelper.GetCurrentUserId(httpClient);
        return user != null && user.Id == userId;
    }
}