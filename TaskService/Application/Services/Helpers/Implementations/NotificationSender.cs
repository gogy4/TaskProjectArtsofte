using System.Net.Http.Json;
using Application.Models;

namespace Application.Services.Helpers.Implementations;

public class NotificationSender(HttpClient httpClient) : INotificationSender
{
    public async Task SendNotificationAsync(int userId, string message)
    {
        var dto = new CreateNotificationDto
        {
            UserId = userId,
            Message = message
        };

        var response = await httpClient.PostAsJsonAsync("", dto);
        response.EnsureSuccessStatusCode();
    }
}
