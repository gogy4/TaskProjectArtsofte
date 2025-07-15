namespace NotificationService.Domain.Entity;

public class Notification
{
    public int Id { get; private set; }
    public int UserId { get; set; }
    public string Message { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsRead { get; set; }

    public Notification(int id, int userId, string message, DateTime createdAt, bool isRead)
    {
        Id = id;
        UserId = userId;
        Message = message;
        CreatedAt = createdAt;
        IsRead = isRead;
    }

    public Notification(int userId, string message, DateTime createdAt, bool isRead)
    {
        UserId = userId;
        Message = message;
        CreatedAt = createdAt;
        IsRead = isRead;
    }
}