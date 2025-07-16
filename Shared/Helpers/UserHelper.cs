using System.Net.Http.Json;
using NotificationService.Application.Models;

namespace Shared.Services;

public class UserHelper
{
    public static async Task<UserIdDto> GetCurrentUserId(HttpClient httpClient)
    {
        var response = await httpClient.GetAsync("me");
        if (!response.IsSuccessStatusCode) throw new ArgumentException("Пользователь не авторизован.");
        var user = await response.Content.ReadFromJsonAsync<UserIdDto>();
        return user;
    }
    
    public static async Task<bool> IsReceiverExists(HttpClient httpClient, int userId)
    {
        var response = await httpClient.GetAsync($"get-user-by/{userId}");
        return response.IsSuccessStatusCode;
    }
}