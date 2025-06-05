using JustInTime.Api.Models;
using JustInTime.Api.Repositories.Categories;

namespace JustInTime.Api.Services.Categories
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _categoryRepository.GetCategoriesAsync();
        }

        public async Task<Guid> GetCategoryId(string categoryName)
        {
            return (await _categoryRepository.GetCategoriesAsync()).ToList().First(category => category.Name == categoryName).Id;
        }
    }
}
