using System.Runtime.InteropServices.ComTypes;
using FluentValidation;
using NotificationService.Application.Models;

namespace NotificationService.Application.Validators;

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
        RuleFor(x=>x.UserId)
            .MustAsync(async (userId, cancellationToken) => await IsReceiverExists(userId))
            .WithMessage("Пользователя не существует.");
    }

    private async Task<bool> IsUserLogin()
    {
        var response = await httpClient.GetAsync("me");
        return response.IsSuccessStatusCode;
    }

    private async Task<bool> IsReceiverExists(int userId)
    {
        var response = await httpClient.GetAsync($"get-user-by/{userId}");
        return response.IsSuccessStatusCode;
    }
}