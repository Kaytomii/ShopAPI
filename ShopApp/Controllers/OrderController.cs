using Microsoft.AspNetCore.Mvc;
using Shop.Application.DTOs.OrdersDTOs;
using Shop.Application.Interfaces.Services;

namespace ShopApi.Controllers;

[ApiController]
[Route("api/orders")]
public class OrderController : ControllerBase
{
    private readonly IQueueService _queueService;

    public OrderController(IQueueService queueService)
    {
        _queueService = queueService;
    }

    [HttpPost("create")]
    public IActionResult CreateOrder([FromBody] OrderCreateDto dto)
    {
        _queueService.Publish("Orders", dto);
        return Ok("Order sent to queue");
    }
}