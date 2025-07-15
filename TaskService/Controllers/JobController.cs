using Microsoft.AspNetCore.Mvc;
using TaskService.Application.Dto;
using TaskService.Application.Services.Abstractions;

namespace TaskService.Controllers;

[ApiController]
[Route("api/tasks")]
[Produces("application/json")]
public class JobController(IJobService service) : ControllerBase
{
    /// <summary>
    /// Получить список задач с фильтрацией и пагинацией.
    /// </summary>
    /// <param name="request">Параметры фильтрации и пагинации.</param>
    /// <returns>Список задач.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAll([FromBody] GetAllJobsRequest request)
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
    /// <param name="id">ID задачи.</param>
    /// <returns>Задача.</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Get(int id)
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
    /// <param name="job">Данные новой задачи.</param>
    /// <returns>Созданная задача.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] JobDto job)
    {
        var result = await service.CreateAsync(job);
        return CreatedAtAction(nameof(Get), new { id = job.Id }, result);
    }

    /// <summary>
    /// Обновить задачу по ID.
    /// </summary>
    /// <param name="id">ID задачи.</param>
    /// <param name="job">Обновленные данные задачи.</param>
    /// <returns>Обновленная задача.</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] JobDto job)
    {
        if (job.Id != id) return BadRequest();
        var result = await service.UpdateAsync(job);
        return Ok(result);
    }

    /// <summary>
    /// Жесткое удаление задачи по ID.
    /// </summary>
    /// <param name="id">ID задачи.</param>
    /// <returns>Статус операции.</returns>
    [HttpDelete("{id}/hard-delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> HardDelete(int id)
    {
        var result = await service.DeleteAsync(id);
        return result == 0 ? NotFound() : Ok();
    }

    /// <summary>
    /// Мягкое удаление задачи по ID (пометка IsDeleted).
    /// </summary>
    /// <param name="id">ID задачи.</param>
    /// <returns>Статус операции.</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> SoftDelete(int id)
    {
        var result = await service.SoftDeleteAsync(id);
        return result == 0 ? NotFound() : Ok();
    }

    /// <summary>
    /// Назначить исполнителя задачи.
    /// </summary>
    /// <param name="id">ID задачи.</param>
    /// <param name="userId">ID пользователя-исполнителя.</param>
    /// <returns>Статус операции.</returns>
    [HttpPut("{id}/assign")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> AssignUser(int id, [FromBody] int userId)
    {
        await service.AssignUserAsync(id, userId);
        return Ok();
    }
}
