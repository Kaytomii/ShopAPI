using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces.Services;
using Shop.Application.DTOs.UserDTOs;

namespace ShopApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]

public class AuthController(IAuthService _authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserCreateDTO dto)
    {
        var user = await _authService.RegisterAsync(dto);

        if (user.User == null || user.Token == null)
            return BadRequest("Користувач за таким email вже існує");

        return Ok(new
        {
            user = user.User,
            token = user.Token
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDTO dto)
    {
        var user = await _authService.LoginAsync(dto);

        if (user.User == null || user.Token == null)
            return Unauthorized("Невірний email або пароль");

        return Ok(new
        {
            user = user.User,
            token = user.Token
        });
    }
}