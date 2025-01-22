using CookingRecipes.App.DTOs.Ingredients;
using CookingRecipes.App.DTOs.Recipes;
using CookingRecipes.App.DTOs.Steps;
using CookingRecipes.App.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookingRecipes.Api.Controllers;

[Route("recipes")]
public class RecipeController : Controller
{
    private readonly IRecipeService _recipeService;
    private readonly IUserService _userService;
    private readonly ICategoryService _categoryService;

    public RecipeController(IRecipeService recipeService, IUserService userService, ICategoryService categoryService)
    {
        _recipeService = recipeService;
        _userService = userService;
        _categoryService = categoryService;
    }

    [HttpGet("create"), Authorize]
    public async Task<IActionResult> Create()
    {
        var categories = await _categoryService.GetAllCategories();
        ViewBag.Categories = categories;
        return View();
    }


    [HttpPost("create"), Authorize]
    public async Task<IActionResult> Create([FromForm] CreateRecipeDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        int currentUserId = _userService.GetCurrentUserId();
        if (currentUserId == 0)
        {
            return RedirectToAction("Login", "User");
        }

        var recipe = await _recipeService.CreateRecipe(dto, currentUserId);

        return RedirectToAction("Details", new { id = recipe.Id });
    }

    [HttpGet("{id}"), Authorize]
    public async Task<IActionResult> Details(int id)
    {
        int currentUserId = _userService.GetCurrentUserId();
        var recipe = await _recipeService.GetRecipeById(id);

        if (recipe == null)
        {
            return NotFound();
        }

        ViewBag.CurrentUserId = currentUserId;
        return View(recipe);
    }

    [HttpGet("edit/{id}"), Authorize]
    public async Task<IActionResult> Edit(int id)
    {
        int currentUserId = _userService.GetCurrentUserId();
        if (currentUserId == 0) return RedirectToAction("Login", "User");

        var recipe = await _recipeService.GetRecipeById(id);
        if (recipe == null || recipe.AuthorId != currentUserId)
        {
            return Forbid();
        }

        var editDto = new EditRecipeDto
        {
            Id = recipe.Id,
            Title = recipe.Title,
            Description = recipe.Description,
            PreparationTime = recipe.PreparationTime,
            CookingTime = recipe.CookingTime,
            Servings = recipe.Servings,
            IsPublic = recipe.IsPublic,
            CurrentImagePath = recipe.ImagePath,

            Ingredients = recipe.Ingredients.Select(i => new CreateIngredientDto
            {
                Name = i.Name,
                Quantity = i.Quantity,
                Unit = i.Unit
            }).ToList(),
            Steps = recipe.Steps.Select(s => new CreateStepDto
            {
                StepNumber = s.StepNumber,
                Instruction = s.Instruction
            }).ToList(),
            CategoryIds = recipe.RecipeCategories.Select(rc => rc.CategoryId).ToList()
        };

        var categories = await _categoryService.GetAllCategories();
        ViewBag.Categories = categories;

        return View(editDto);
    }

    [HttpPost("edit/{id}"), Authorize]
    public async Task<IActionResult> Edit(int id, [FromForm] EditRecipeDto dto)
    {
        int currentUserId = _userService.GetCurrentUserId();
        if (currentUserId == 0) return RedirectToAction("Login", "User");

        dto.Id = id;

        if (!ModelState.IsValid)
        {
            var categories = await _categoryService.GetAllCategories();
            ViewBag.Categories = categories;
            return View(dto);
        }

        var updated = await _recipeService.UpdateRecipe(dto, currentUserId);
        if (!updated)
        {
            return Forbid();
        }

        return RedirectToAction("Details", new { id = dto.Id });
    }

    [HttpPost("delete"), Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        int currentUserId = _userService.GetCurrentUserId();
        if (currentUserId == 0)
        {
            return RedirectToAction("Login", "User");
        }

        bool deleted = await _recipeService.DeleteRecipe(id, currentUserId);
        if (!deleted)
        {
            return Forbid();
        }

        return RedirectToAction("Index", "Home");
    }


}
