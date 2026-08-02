using Microsoft.AspNetCore.Mvc;
using ShopApi.Interfaces;
using Shop.Application.Interfaces.Services;
using Shop.Application.DTOs.CategoryDTOs;
using ShopApi.Requests.Categories;


namespace ShopApi.Controllers;


[ApiController]
[Route("api/v1/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryTreeService _treeService;

    public CategoryController(ICategoryTreeService treeService)
    {
        _treeService = treeService;
    }

    [HttpGet("{id:int}/parents")]
    public async Task<IActionResult> GetParents(int id)
    {
        var parents = await _treeService.GetParentCategoriesAsync(id);
        return Ok(parents);
    }

    [HttpGet("{id:int}/children")]
    public async Task<IActionResult> GetChildren(int id)
    {
        var children = await _treeService.GetChildCategoriesAsync(id);
        return Ok(children);
    }

    [HttpGet("tree")]
    public async Task<IActionResult> GetTree()
    {
        var tree = await _treeService.GetCategoryTreeAsync();
        return Ok(tree);
    }
}