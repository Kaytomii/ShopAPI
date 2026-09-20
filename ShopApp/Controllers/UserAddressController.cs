using Microsoft.AspNetCore.Mvc;
using Shop.Application.DTOs.UserDTOs;
using Shop.Application.Services;

namespace ShopApi.Controllers;

[ApiController]
[Route("api/user/address")]
public class UserAddressController : ControllerBase
{
    private readonly UserAddressService _service;

    public UserAddressController(UserAddressService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> AddAddress(
        [FromBody] UserAddressCreateDto dto,
        CancellationToken token)
    {
        await _service.AddAddressAsync(dto, token);
        return Ok("Address added");
    }

    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetAddresses(Guid userId, CancellationToken token)
    {
        var addresses = await _service.GetUserAddressesAsync(userId, token);
        return Ok(addresses);
    }
}