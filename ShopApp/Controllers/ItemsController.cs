using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace ShopApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController:ControllerBase
{
    [HttpGet]
    public string GetItem()
    {
        return "New Items";
    }

    [HttpGet("{id}")]
    public string GetItemById([FromRoute] int id)
    {
        return $"Item id: {id}";
    }
}
