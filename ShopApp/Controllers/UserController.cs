using Microsoft.AspNetCore.Mvc;
using ShopDomain.Models;
using ShopApi.Interfaces;
using ShopApi.Filters;
namespace ShopApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[UserFilter]
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
