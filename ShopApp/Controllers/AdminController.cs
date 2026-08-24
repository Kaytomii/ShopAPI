using Microsoft.AspNetCore.Mvc;
using Shop.Application.Interfaces.Services;

namespace ShopApi.Controllers;

[ApiController]
[Route("api/admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpPost("create-admin")]
    public async Task<IActionResult> CreateAdmin(string email, string password)
    {
        var admin = await _adminService.CreateAdminAsync(email, password);
        return Ok(admin);
    }

    [HttpPost("create-moderator")]
    public async Task<IActionResult> CreateModerator(string email, string password)
    {
        var moderator = await _adminService.CreateModeratorAsync(email, password);
        return Ok(moderator);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword(Guid userId, string newPassword)
    {
        var result = await _adminService.ResetUserPasswordAsync(userId, newPassword);
        if (!result)
            return NotFound("User not found");

        return Ok("Password updated");
    }
}