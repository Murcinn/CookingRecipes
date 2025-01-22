using CookingRecipes.App.DTOs.Categories;
using CookingRecipes.App.Interfaces;
using CookingRecipes.Domain.Entities;
using CookingRecipes.Domain.Interfaces;

namespace CookingRecipes.App.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<Category>> GetAllCategories()
    {
        return await _categoryRepository.GetAll();
    }

    public async Task AddCategory(AddCategoryDto dto)
    {
        var category = new Category(dto.Name);
        await _categoryRepository.Add(category);
    }

    public async Task DeleteCategory(int id)
    {
        await _categoryRepository.Delete(id);
    }
}
