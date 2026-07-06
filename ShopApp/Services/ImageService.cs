using ShopApi.Interfaces;
using ShopApi.Enums;
namespace ShopApi.Services;

public class ImageService(IWebHostEnvironment _environment) : IImageService
{
    private static string _dirname = "categories";

    public async Task<string> SaveFileAsync(IFormFile file, ImageDirectoryEnum directory)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("File is empty.");

        var folderName = directory.ToString();
        var folderPath = Path.Combine(_environment.WebRootPath, folderName);

        Directory.CreateDirectory(folderPath);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(folderPath, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(stream);

        return fileName;
    }
}