using Microsoft.AspNetCore.Mvc;
using ShopDomain.Models;

namespace ShopApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController:ControllerBase
    {
        private List<Product> _products = new();
        [HttpGet]
        public List<Product> GetProducts()
        {
            _products.Add(new Product()
            {
                Title = "Milk",
                Price = 44.6f
            });

            _products.Add(new Product()
            {
                Title = "Bread",
                Price = 44.6f
            });
            return _products;

        }
    }
}
