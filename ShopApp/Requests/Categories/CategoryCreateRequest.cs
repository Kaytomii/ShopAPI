using Shop.Application.DTOs.CategoryDTOs;

namespace ShopApi.Requests.Categories;

public class CategoryCreateRequest : CategoryCreateDTO
{
    public IFormFile? Image { get; set; }
}
