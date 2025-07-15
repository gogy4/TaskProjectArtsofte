using AuthService.Application.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AuthService.Controllers;

[ApiController]
[Route("api/auth")]
public class UserController(IUserService service) : ControllerBase
{
    /// <summary>
    /// Регистрация нового пользователя.
    /// </summary>
    /// <param name="request">Данные для регистрации (логин и пароль).</param>
    /// <returns>Id зарегистрированного пользователя.</returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] AuthRequest request)
    {
        try
        {
            var id = await service.RegisterAsync(request);
            return Ok(id);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Вход пользователя в систему.
    /// </summary>
    /// <param name="request">Данные для входа (логин и пароль).</param>
    /// <returns>JWT токен для аутентификации.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthRequest request)
    {
        try
        {
            var token = await service.LoginAsync(request);
            return Ok(token);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Получение информации о текущем пользователе.
    /// </summary>
    /// <returns>Данные текущего пользователя.</returns>
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        try
        {
            var user = await service.GetCurrentUser();
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    /// <summary>
    /// Выход пользователя из системы.
    /// </summary>
    /// <returns>Статус выхода.</returns>
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        try
        {
            service.Logout(HttpContext);
            return Ok("Успешный выход");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Получение пользователя по Id.
    /// </summary>
    /// <param name="id">Id пользователя.</param>
    /// <returns>Данные пользователя с указанным Id.</returns>
    [HttpGet("get-user-by/{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        try
        {
            var user = await service.GetUserByIdAsync(id);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
