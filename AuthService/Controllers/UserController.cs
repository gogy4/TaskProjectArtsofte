using AuthService.Application.Models;
using AuthService.Application.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers;

[ApiController]
[Route("api/auth")]
public class UserController(IUserService service) : ControllerBase
{
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

    [HttpPost("me")]
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