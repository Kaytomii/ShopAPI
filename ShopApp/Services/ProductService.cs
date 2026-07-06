using ShopApi.Interfaces;
using ShopDomain.Models;
namespace ShopApi.Services
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
                Name = "Milk",
                Price = 44.6m
            });

            _products.Add(new Product()
            {
                Name = "Bread",
                Price = 44.6m
            });
            return _products;

        }
    }
}
