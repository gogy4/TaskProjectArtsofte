using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Application.Models;
using NotificationService.Application.Services.Abstractions;

namespace NotificationService.Controllers;

[ApiController]
[Route("api/notifications")]
public class NotificationsController(INotificationService service) : ControllerBase
{
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetNotifications(int userId)
    {
        try
        {
            var notifs = await service.GetNotifications(userId);
            return Ok(notifs);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> SendNotification(CreateNotificationDto dto)
    {
        try
        {
            var id =await service.SendNotification(dto);
            return Ok(id);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}/mark-as-read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        try
        {
            var resultId = await service.MarkAsRead(id);
            return Ok(resultId);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}