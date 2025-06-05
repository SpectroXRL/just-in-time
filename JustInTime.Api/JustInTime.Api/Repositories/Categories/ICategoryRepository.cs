using JustInTime.Api.Models;

namespace JustInTime.Api.Repositories.Categories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategoriesAsync();
        Task<Category> GetCategoryAsync(Guid id);
    }
}