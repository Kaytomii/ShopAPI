using Microsoft.AspNetCore.Mvc;
using ShopApi.Interfaces;
using Shop.Application.Interfaces.Services;
using Shop.Application.DTOs.CategoryDTOs;
using ShopApi.Requests.Categories;


namespace ShopApi.Controllers;


[ApiController]
[Route("api/v1/[controller]")]
public class CategoryController(ICategoryService _categoryService, IImageService _imageService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromForm] CategoryCreateRequest dto)
    {
        if (dto.Image != null)

        {

            dto.Url = (await _imageService.SaveFileAsync(dto.Image, Enums.ImageDirectoryEnum.Categories)) ?? string.Empty;

        }


        var createDto = new CategoryCreateDTO
        {

            Name = dto.Name,

            Url = dto.Url,

            Slug = dto.Slug,

            ParentId = dto.ParentId,

        };


        var id = await _categoryService.CreateCategoryAsync(createDto);
        return Ok($"Category created {id}");
    }
}