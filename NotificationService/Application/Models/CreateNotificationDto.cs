namespace NotificationService.Application.Models;

public class CreateNotificationDto
{
    public int UserId { get; set; }
    public string Message { get; set; }

    public CreateNotificationDto(int userId, string message)
    {
        UserId = userId;
        Message = message;
    }
}