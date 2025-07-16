using Domain.Entity;

namespace Infrastructure.Repository.Abstractions;

public interface INotificationRepository
{
    Task<IEnumerable<Notification>> GetByUserIdAsync(int userId);
    Task<int> AddAsync(Notification notification);
    Task MarkAsReadAsync(int id);
    Task<Notification> GetByIdAsync(int id);
}