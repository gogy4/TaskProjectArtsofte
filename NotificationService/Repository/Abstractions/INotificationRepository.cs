using NotificationService.Domain.Entity;

namespace NotificationService.Repository.Abstractions;

public interface INotificationRepository
{
    Task<IEnumerable<Notification>> GetByUserIdAsync(int userId);
    Task<Notification> AddAsync(Notification notification);
    Task MarkAsReadAsync(int id);
}