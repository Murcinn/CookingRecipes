using CookingRecipes.App.DTOs.Categories;
using CookingRecipes.App.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookingRecipes.Api.Controllers;

[Authorize(Policy = "AdminPolicy")]
[Route("admin")]
public class AdminController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly IUserService _userService;

    public AdminController(ICategoryService categoryService, IUserService userService)
    {
        _categoryService = categoryService;
        _userService = userService;
    }

    [HttpGet("category")]
    public async Task<IActionResult> Category()
    {
        var categories = await _categoryService.GetAllCategories();
        return View("~/Views/Admin/Category.cshtml", categories);
    }


    [HttpPost]
    public async Task<IActionResult> AddCategory(AddCategoryDto dto)
    {
        if (ModelState.IsValid)
        {
            await _categoryService.AddCategory(dto);
            return RedirectToAction("Category");
        }
        return View("~/Views/Admin/Category.cshtml", await _categoryService.GetAllCategories());
    }

    [HttpPost("delete/category/{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        await _categoryService.DeleteCategory(id);
        return RedirectToAction("category");
    }

    [HttpGet("user")]
    public async Task<IActionResult> Users()
    {
        var users = await _userService.GetAllUsers();
        return View("~/Views/Admin/User.cshtml", users);
    }

    [HttpPost("delete/user/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        await _userService.DeleteUser(id);
        return RedirectToAction("User");
    }

    [HttpPost("restore/user/{id}")]
    public async Task<IActionResult> RestoreUser(int id)
    {
        await _userService.RestoreUser(id);
        return RedirectToAction("User");
    }
}
