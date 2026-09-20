using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shop.Application.DTOs.ProductDTOs;
using Shop.Application.Interfaces.Services;
using Shop.Application.Queries.Product;
using Shop.Infrastructure.Services;
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
        private readonly Shop.Application.Interfaces.Services.IProductService _service;
        private readonly IImageService _imageService;
        private readonly IMediator _mediator;
        private readonly ProductFeedbackService _feedbackService;

        public ProductController(Shop.Application.Interfaces.Services.IProductService service, IImageService imageService, IMediator mediator, ProductFeedbackService feedbackService)
        {
            _service = service;
            _imageService = imageService;
            _mediator = mediator;
            _feedbackService = feedbackService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProductCreateRequest req)
        {
            var urls = new List<string>();

            if (req.Images != null)
            {
                foreach (var img in req.Images)
                {
                    var url = await _imageService.SaveFileAsync(img, ShopApi.Enums.ImageDirectoryEnum.Categories);
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
            var product = await _mediator.Send(new GetProductByIdQuery(id));
            if (product == null) return NotFound();

            return Ok(product);
        }
        [HttpPost("feedback")]
        public async Task<IActionResult> AddFeedback([FromBody] ProductFeedbackDto dto)
        {
            var feedback = new ProductFeedback
            {
                ProductId = dto.ProductId,
                UserId = dto.UserId,
                Type = dto.Type,
                Message = dto.Message,
                Rating = dto.Rating
            };

            await _feedbackService.AddAsync(feedback);

            return Ok("Feedback saved to MongoDB");
        }
    }
}
