using Application.Models;
using Application.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace NotificationApi.Controllers;

/// <summary>
/// Управление уведомлениями пользователей.
/// </summary>
[ApiController]
[Route("api/notifications")]
public class NotificationsController(INotificationService service) : ControllerBase
{
    /// <summary>
    /// Получить уведомления для пользователя по его ID.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя.</param>
    /// <returns>Список уведомлений пользователя.</returns>
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

    /// <summary>
    /// Отправить новое уведомление.
    /// </summary>
    /// <param name="dto">Данные уведомления для создания.</param>
    /// <returns>Идентификатор созданного уведомления.</returns>
    [HttpPost]
    public async Task<IActionResult> SendNotification(CreateNotificationDto dto)
    {
        try
        {
            var id = await service.SendNotification(dto);
            return Ok(id);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Отметить уведомление как прочитанное по его ID.
    /// </summary>
    /// <param name="id">Идентификатор уведомления.</param>
    /// <returns>Идентификатор обработанного уведомления.</returns>
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