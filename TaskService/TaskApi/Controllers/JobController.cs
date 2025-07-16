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
    /// Получить список задач с фильтрацией и пагинацией.
    /// </summary>
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
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateJobRequest job)
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
    [HttpDelete("{id}/hard-delete")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> HardDelete(int id)
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
    public async Task<IActionResult> AssignUser(int id, [FromBody] int userId)
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
    /// Получить всю историю изменений для job по jobId
    /// </summary>
    /// <param name="jobId"></param>
    /// <returns></returns>
    [HttpGet("get-history-by-job/{jobId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetHistoryByJobId(int jobId)
    {
        try
        {
            var history = await service.GetHistoryByJobIdAsync(jobId);
            return Ok(history);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}