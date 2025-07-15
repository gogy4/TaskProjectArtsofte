using Microsoft.AspNetCore.SignalR;
using NotificationService.Application.Models;
using NotificationService.Application.Services.Abstractions;
using NotificationService.Domain.Entity;
using NotificationService.Hubs;
using NotificationService.Repository.Abstractions;

namespace NotificationService.Application.Services.Implementations;

public class NotificationService(INotificationRepository repository, IHubContext<NotificationHub> hub) : INotificationService
{
    public async Task<IEnumerable<NotificationDto>> GetNotifications(int userId)
    {
        var notifs = await repository.GetByUserIdAsync(userId);
        return notifs.Select(n => new NotificationDto(n.Id, n.Message, n.CreatedAt, n.IsRead));
    }

    public async Task<int> SendNotification(CreateNotificationDto dto)
    {
        var notif = new Notification(dto.UserId, dto.Message, DateTime.UtcNow, false);
        await repository.AddAsync(notif);
        await hub.Clients.User(dto.UserId.ToString())
            .SendAsync("ReceiveNotification", dto.Message);

        return notif.Id;
    }

    public async Task<int> MarkAsRead(int id)
    {
        await repository.MarkAsReadAsync(id);
        return id;
    }
}