namespace Application.Services.Helpers;

public interface INotificationSender
{
    Task SendNotificationAsync(int userId, string message);
}
