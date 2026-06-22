using Microsoft.AspNetCore.Mvc;
using ShopApp.Services;
using ShopApp.Interfaces;
using ShopDomain.Models;
namespace ShopApp.Controllers;


[ApiController]
[Route("api/[controller]")]
public class CategoryController(ICategoryService _categoryService) : ControllerBase
{
    [HttpGet]
    public List<Category> GetCategories()
    {
        return _categoryService.GetAllCategories();
    }
}