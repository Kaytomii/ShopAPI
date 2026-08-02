namespace ShopApi.Requests.Products;

public class ProductCreateRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int CategoryId { get; set; }

    public List<IFormFile>? Images { get; set; }
}
