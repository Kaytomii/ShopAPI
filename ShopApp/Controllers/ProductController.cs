using Microsoft.AspNetCore.Mvc;
using ShopApp.Interfaces;
using ShopDomain.Models;

namespace ShopApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController:ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public List<Product> GetProducts()
        {
            return _productService.GetAllProducts();
        }

        [HttpPost]
        public IActionResult AddNewProduct([FromBody] Product product)
        {
            _productService.AddProduct(product);
            return Created();
        }
    }
}
