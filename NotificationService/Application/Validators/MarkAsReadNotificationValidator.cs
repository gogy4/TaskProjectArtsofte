using FluentValidation;
using NotificationService.Application.Models;

namespace NotificationService.Controllers;

public class MarkAsReadNotificationValidator : AbstractValidator<NotificationDto>
{
    private readonly HttpClient httpClient;
    
    public MarkAsReadNotificationValidator(HttpClient httpClient)
    {
        this.httpClient = httpClient;
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required");
        RuleFor(x=>x.UserId)
            .MustAsync(async (userId, cancellationToken) => await IsUserLogin(userId))
            .WithMessage("Пользователь не авторизован или это сообщение принадлежит другому пользователю");
    }
    
    private async Task<bool> IsUserLogin(int userId)
    {
        var response = await httpClient.GetAsync("me");
        if (!response.IsSuccessStatusCode) return false;

        var user = await response.Content.ReadFromJsonAsync<UserIdDto>();
        return user != null && user.Id == userId;
    }
}