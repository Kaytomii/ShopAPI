using ShopApi.Enums;

namespace ShopApi.Interfaces;

public interface IImageService
{
    Task<string> SaveFileAsync(IFormFile file, ImageDirectoryEnum directory);
}
