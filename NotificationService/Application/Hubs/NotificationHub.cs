using Microsoft.AspNetCore.SignalR;

namespace NotificationService.Hubs;

public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        Console.WriteLine($"User connected: {userId}");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        var userId = Context.UserIdentifier;
        Console.WriteLine($"User disconnected: {userId}");
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendNotification(string userId, string message)
    {
        await Clients.User(userId).SendAsync("ReceiveNotification", message);
    }
}