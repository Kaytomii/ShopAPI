using Microsoft.AspNetCore.Mvc;
using Shop.Application.DTOs.CategoryDTOs;
using Shop.Application.Interfaces.Services;
using Shop.Application.Queries.Category;
using ShopApi.Interfaces;
using ShopApi.Requests.Categories;
using MediatR;


namespace ShopApi.Controllers;


[ApiController]
[Route("api/v1/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryTreeService _treeService;
    private readonly IMediator _mediator;

    public CategoryController(ICategoryTreeService treeService, IMediator mediator)
    {
        _treeService = treeService;
        _mediator = mediator;
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
    [HttpGet("{slug}")]
    public async Task<ActionResult<CategoryReadDTO>> GetCategoryBySlug(string slug)
    {
        var dto = await _mediator.Send(new GetCategoryBySlugQuery(slug));

        if (dto == null)
            return NotFound();

        return Ok(dto);
    }
}