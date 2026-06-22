using ShopApp.Interfaces;
using ShopDomain.Models;
namespace ShopApp.Services
{
    public class ProductService : IProductService
    {
        private List<Product> _products = new();

        public void AddProduct(Product product)
        {
            _products.Add(product);
        }

        public List<Product> GetAllProducts()
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
