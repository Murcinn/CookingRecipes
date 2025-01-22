using CookingRecipes.Api.Models;
using CookingRecipes.App.Interfaces;
using CookingRecipes.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CookingRecipes.Controllers;

public class HomeController : Controller
{
    private readonly IRecipeService _recipeService;
    private readonly IUserService _userService;
    private readonly ICategoryService _categoryService;

    public HomeController(IRecipeService recipeService, IUserService userService, ICategoryService categoryService)
    {
        _recipeService = recipeService;
        _userService = userService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index(string? searchQuery, int? categoryId)
    {
        int currentUserId = _userService.GetCurrentUserId();
        var categories = await _categoryService.GetAllCategories();

        List<Recipe> allRecipes = await _recipeService.GetAllPublicRecipes();

        if (!string.IsNullOrEmpty(searchQuery))
        {
            allRecipes = allRecipes.FindAll(r => r.Title.Contains(searchQuery, System.StringComparison.OrdinalIgnoreCase));
        }

        if (categoryId.HasValue)
        {
            allRecipes = allRecipes.FindAll(r =>
                r.RecipeCategories.Any(rc => rc.CategoryId == categoryId.Value));
        }

        List<Recipe> myRecipes = new();
        if (currentUserId != 0)
        {
            myRecipes = await _recipeService.GetRecipesByAuthor(currentUserId);

            if (!string.IsNullOrEmpty(searchQuery))
            {
                myRecipes = myRecipes.FindAll(r => r.Title.Contains(searchQuery, System.StringComparison.OrdinalIgnoreCase));
            }

            if (categoryId.HasValue)
            {
                myRecipes = myRecipes.FindAll(r =>
                    r.RecipeCategories.Any(rc => rc.CategoryId == categoryId.Value));
            }
        }

        var model = new HomeRecipesModel
        {
            SearchQuery = searchQuery,
            SelectedCategoryId = categoryId,
            Categories = categories,
            AllRecipes = allRecipes,
            MyRecipes = myRecipes
        };

        return View(model);
    }
}
