using CookingRecipes.Domain.Entities;

namespace CookingRecipes.Domain.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetAll();
    Task Add(Category category);
    Task Delete(int id);
}
