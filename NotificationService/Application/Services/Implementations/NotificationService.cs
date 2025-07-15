using FluentValidation;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Application.Mappers;
using NotificationService.Application.Models;
using NotificationService.Application.Services.Abstractions;
using NotificationService.Controllers;
using NotificationService.Domain.Entity;
using NotificationService.Hubs;
using NotificationService.Repository.Abstractions;

namespace NotificationService.Application.Services.Implementations;

public class NotificationService(
    INotificationRepository repository,
    IHubContext<NotificationHub> hub,
    IValidator<CreateNotificationDto> createValidator,
    IValidator<NotificationDto> markAsReadValidator) : INotificationService
{
    public async Task<IEnumerable<NotificationDto>> GetNotifications(int userId)
    {
        var notifs = await repository.GetByUserIdAsync(userId);
        return notifs.Select(NotificationMapper.ToDto);
    }

    public async Task<int> SendNotification(CreateNotificationDto dto)
    {
        var result = await createValidator.ValidateAsync(dto);
        if (!result.IsValid)
        {
            throw new ValidationException(string.Join(", ", result.Errors.Select(e => e.ErrorMessage)));
        }
        var notif = new Notification(dto.UserId, dto.Message, DateTime.UtcNow, false);
        var id = await repository.AddAsync(notif);
        await hub.Clients.User(dto.UserId.ToString())
            .SendAsync("ReceiveNotification", dto.Message);

        return id;
    }

    public async Task<int> MarkAsRead(int id)
    {
        var notification = await repository.GetByIdAsync(id);
        var dto = NotificationMapper.ToDto(notification);
        var result = await markAsReadValidator.ValidateAsync(dto);
        if (!result.IsValid)
        {
            throw new ValidationException(string.Join(", ", result.Errors.Select(e => e.ErrorMessage)));
        }
        await repository.MarkAsReadAsync(id);
        return id;
    }
}