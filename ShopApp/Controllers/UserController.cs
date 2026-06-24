using Microsoft.AspNetCore.Mvc;
using ShopDomain.Models;
using ShopApp.Interfaces;

namespace ShopApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService _userService) : ControllerBase
{
    [HttpPost("register")]
    public IActionResult AddNewUser([FromBody]User user)
    {
        Console.WriteLine($"User: {user.id}, {user.Login}, {user.Email}");
        _userService.AddUser(user);
        return Created();
    }
}
