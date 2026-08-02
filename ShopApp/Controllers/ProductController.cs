using Microsoft.AspNetCore.Mvc;
using Shop.Application.DTOs.ProductDTOs;
using ShopApi.Enums;
using ShopApi.Interfaces;
using ShopApi.Requests.Products;
using ShopDomain.Models;

namespace ShopApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        private readonly IImageService _imageService;

        public ProductController(IProductService service, IImageService imageService)
        {
            _service = service;
            _imageService = imageService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest req)
        {
            var urls = new List<string>();

            if (req.Images != null)
            {
                foreach (var img in req.Images)
                {
                    var url = await _imageService.SaveFileAsync(img, ImageDirectoryEnum.Products);
                    if (url != null)
                        urls.Add(url);
                }
            }

            var dto = new ProductCreateDTO
            {
                Name = req.Name,
                Description = req.Description,
                Price = req.Price,
                CategoryId = req.CategoryId,
                ImageUrls = urls
            };

            var id = await _service.CreateProductAsync(dto);
            return Ok(id);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _service.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);
            if (product == null) return NotFound();

            return Ok(product);
        }
    }
}
