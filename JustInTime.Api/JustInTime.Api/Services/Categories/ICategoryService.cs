using JustInTime.Api.Models;

namespace JustInTime.Api.Services.Categories
{
    public interface ICategoryService
    {
        Task<List<Category>> GetCategoriesAsync();
        Task<Guid> GetCategoryId(string categoryName);
    }
}