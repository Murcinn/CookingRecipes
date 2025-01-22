using CookingRecipes.App.DTOs.Categories;
using CookingRecipes.Domain.Entities;

namespace CookingRecipes.App.Interfaces;

public interface ICategoryService
{
    Task<List<Category>> GetAllCategories();
    Task AddCategory(AddCategoryDto dto);
    Task DeleteCategory(int id);
}
