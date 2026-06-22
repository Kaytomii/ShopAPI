using ShopApp.Interfaces;
using ShopDomain.Models;

namespace ShopApp.Services
{
    public class CategoryService : ICategoryService
    {
        private List<Category> _categories = new();
        public List<Category> GetAllCategories()
        {
            _categories.Add(new Category()
            {
                Id = 1,
                Title = "Technology",
                Description = "All about modern tech and innovations",
                Image = "tech.jpg",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsShow = true
            });
            _categories.Add(new Category()
            {
                Id = 2,
                Title = "Travel",
                Description = "Places, trips and travel guides",
                Image = "travel.jpg",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsShow = true
            });

            _categories.Add(new Category()
            {
                Id = 3,
                Title = "Food",
                Description = "Recipes, dishes and food culture",
                Image = "food.jpg",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsShow = true
            });

            return _categories;
        }
    }
}
