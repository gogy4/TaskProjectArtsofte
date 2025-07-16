using Application.Models;

namespace Application.Services.Abstractions;

public interface INotificationService
{
    Task<IEnumerable<NotificationDto>> GetNotifications(int userId);
    Task<int> SendNotification(CreateNotificationDto dto);
    Task<int> MarkAsRead(int id);
}