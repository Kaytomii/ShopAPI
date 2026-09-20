using Microsoft.AspNetCore.Mvc;
using Shop.Application.DTOs.UserDTOs;
using Shop.Application.Interfaces.Services;
using Shop.Application.Interfaces.Repository;
using Shop.Infrastructure.Services;
using ShopDomain.Enums;
using ShopDomain.Models;

namespace ShopApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(
    IAuthService _authService,
    IUserProviderRepository _userProviderRepository,
    IJWTService _jwtService) : ControllerBase
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
    [HttpPost("external")]
    public async Task<IActionResult> ExternalResponse([FromBody] ExternalAuthDTO dto, CancellationToken token)
    {
        var user = await _authService.GetUserByEmailAsync(dto.Email);

        if (user == null)
        {
            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Email = dto.Email,
                Role = UserRole.User
            };

            await _authService.CreateExternalUserAsync(newUser);

            user = newUser;
        }

        var provider = await _userProviderRepository.GetByNameAsync(dto.Provider);
        if (provider == null)
            return BadRequest("Unknown provider");

        var exists = await _userProviderRepository.ExistsAsync(user.Id, provider.Id);

        if (!exists)
        {
            var up = new UserProvider
            {
                UserId = user.Id,
                ProviderId = provider.Id,
                NumberProvider = dto.ProviderUserId
            };

            await _userProviderRepository.AddAsync(up);
        }

        var accessToken = _jwtService.GenerateAccessToken(
            new UserLoginDTO { Email = user.Email },
            user.Role.ToString()
        );

        var refreshToken = await _authService.GenerateRefreshTokenAsync(user.Id);

        Response.Cookies.Append("refresh_token", refreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(3)
        });

        return Ok(new
        {
            user = new { user.Id, user.Email },
            access_token = accessToken
        });
    }
}
