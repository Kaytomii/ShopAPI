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
        var result = await _authService.RegisterAsync(dto);

        if (result.User == null || result.AccessToken == null || result.RefreshToken == null)
            return BadRequest("Користувач за таким email вже існує");

        Response.Cookies.Append("refresh_token", result.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(3)
        });

        return Ok(new
        {
            user = result.User,
            access_token = result.AccessToken
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDTO dto)
    {
        var result = await _authService.LoginAsync(dto);

        if (result.User == null || result.AccessToken == null || result.RefreshToken == null)
            return Unauthorized("Невірний email або пароль");

        Response.Cookies.Append("refresh_token", result.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(3)
        });

        return Ok(new
        {
            user = result.User,
            access_token = result.AccessToken
        });
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh()
    {
        var refreshToken = Request.Cookies["refresh_token"];
        if (refreshToken == null)
            return Unauthorized("Refresh token not found");

        var result = await _authService.RefreshAsync(refreshToken);
        if (result == null)
            return Unauthorized("Invalid or expired refresh token");

        return Ok(new
        {
            access_token = result
        });
    }
}
