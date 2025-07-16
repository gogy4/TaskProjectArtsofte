using Application.Models;
using Application.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace TaskApi.Controllers;

[ApiController]
[Route("api/tasks")]
[Produces("application/json")]
public class JobController(IJobService service) : ControllerBase
{
    /// <summary>
    /// Получить список задач по текущему пользователю с фильтрацией и пагинацией.
    /// </summary>
    /// <remarks>
    /// Поддерживается фильтрация по заголовку задачи (filter — поиск по title).
    /// Для пагинации используются параметры:
    /// - pageSize — количество задач, отображаемых на странице,
    /// - pageNumber — номер страницы для отображения.
    /// </remarks>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllJobsRequest request)
    {
        try
        {
            var result = await service.GetAllAsync(request);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Получить задачу по ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get([FromRoute] int id)
    {
        try
        {
            var job = await service.GetByIdAsync(id);
            return Ok(job);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Создать новую задачу.
    /// </summary>
    /// <remarks>
    /// При создании задачи отправляется уведомление пользователю, указанному как создатель задачи, 
    /// а также назначенному исполнителю (если он указан).
    /// </remarks>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] JobDto job)
    {
        try
        {
            var result = await service.CreateAsync(job);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Обновить задачу по ID.
    /// </summary>
    /// <remarks>
    /// При обновлении задачи отправляется уведомление пользователю, указанному как создатель задачи, 
    /// а также текущему исполнителю (если он назначен). 
    /// Это информирует их об изменениях в задаче.
    /// </remarks>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateJobRequest job)
    {
        try
        {
            var result = await service.UpdateAsync(id, job);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Жесткое удаление задачи по ID.
    /// </summary>
    /// <remarks>
    /// При жестком удалении задачи отправляется уведомление пользователю, указанному как создатель задачи, 
    /// а также исполнителю (если назначен), с информацией об удалении задачи.
    /// </remarks>
    [HttpDelete("{id}/hard-delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> HardDelete([FromRoute] int id)
    {
        try
        {
            var result = await service.DeleteAsync(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Мягкое удаление задачи по ID (IsDeleted).
    /// </summary>
    /// <remarks>
    /// При мягком удалении задачи (логическом удалении) отправляется уведомление пользователю, указанному как создатель задачи,
    /// а также исполнителю (если назначен), чтобы информировать об изменении статуса задачи.
    /// </remarks>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> SoftDelete(int id)
    {
        try
        {
            var result = await service.SoftDeleteAsync(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Назначить исполнителя задачи.
    /// </summary>
    [HttpPut("{id}/assign")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AssignUser([FromRoute] int id, [FromBody] int userId)
    {
        try
        {
            var result = await service.AssignUserAsync(id, userId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Получить всю историю изменений для задачи по jobId.
    /// </summary>
    /// <remarks>
    /// Поддерживается пагинация с параметрами:
    /// - pageSize — количество записей истории на странице,
    /// - pageNumber — номер страницы.
    /// </remarks>
    /// <param name="jobId">ID задачи, для которой запрашивается история изменений.</param>
    [HttpGet("get-history-by-job/{jobId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetHistoryByJobId([FromRoute] int jobId, [FromQuery] GetAllHistoryRequest request)
    {
        try
        {
            var history = await service.GetHistoryByJobIdAsync(jobId, request.PageNumber, request.PageSize);
            return Ok(history);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}