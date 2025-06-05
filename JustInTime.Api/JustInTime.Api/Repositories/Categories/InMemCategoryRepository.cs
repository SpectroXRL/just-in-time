using JustInTime.Api.Models;

namespace JustInTime.Api.Repositories.Categories
{
    public class InMemCategoryRepository : ICategoryRepository
    {
        public readonly List<Category> categories;

        public InMemCategoryRepository()
        {
            categories = new()
            {
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Entertainment"
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Music"
                },
            };
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await Task.FromResult(categories);
        }

        public async Task<Category> GetCategoryAsync(Guid id)
        {
            return await Task.FromResult(categories.First(category => category.Id == id));
        }
    }
}
