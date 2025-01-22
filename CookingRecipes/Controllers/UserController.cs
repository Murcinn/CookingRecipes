using CookingRecipes.App.DTOs.Users;
using CookingRecipes.App.Interfaces;
using CookingRecipes.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CookingRecipes.Api.Controllers;

[Route("users")]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("register"), AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost("register"), AllowAnonymous]
    public async Task<IActionResult> Register([FromForm] RegisterUserDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var user = new User(dto.FirstName, dto.LastName, dto.DateOfBirth, dto.Email);
        var result = await _userService.Register(user, dto.Password);

        if (!result)
        {
            ModelState.AddModelError("", "Email is already registered.");
            return View(dto);
        }

        return RedirectToAction("Login");
    }

    [HttpGet("login"), AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost("login"), AllowAnonymous]
    public async Task<IActionResult> Login([FromForm] LoginUserDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var token = await _userService.Login(dto.Email, dto.Password);

        if (token == null)
        {
            ModelState.AddModelError("", "Invalid credentials.");
            return View(dto);
        }

        var cookieOptions = new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(7),
            HttpOnly = true,
            IsEssential = true
        };
        Response.Cookies.Append("AuthToken", token, cookieOptions);

        return RedirectToAction("Index", "Home");
    }


    [HttpGet("logout")]
    public IActionResult Logout()
    {
        if (Request.Cookies.ContainsKey("AuthToken"))
        {
            Response.Cookies.Delete("AuthToken");
        }
        return RedirectToAction("Login");
    }


    [HttpGet("edit")]
    public async Task<IActionResult> EditProfile()
    {
        var authToken = HttpContext.Request.Cookies["AuthToken"];
        if (string.IsNullOrEmpty(authToken))
        {
            return RedirectToAction("Login");
        }

        int userId = _userService.GetCurrentUserId();
        if (userId == 0)
        {
            return RedirectToAction("Login");
        }

        var userDto = await _userService.GetUserForEdit(userId);
        if (userDto == null)
        {
            return RedirectToAction("Login");
        }

        return View("EditProfile", userDto);
    }

    [HttpPost("edit")]
    public async Task<IActionResult> EditProfile([FromForm] EditUserDto dto)
    {
        var authToken = HttpContext.Request.Cookies["AuthToken"];
        if (string.IsNullOrEmpty(authToken))
        {
            return RedirectToAction("Login");
        }

        int userId = _userService.GetCurrentUserId();
        if (userId == 0)
        {
            return RedirectToAction("Login");
        }

        if (!ModelState.IsValid)
        {
            return View("EditProfile", dto);
        }

        var updated = await _userService.UpdateUserProfile(userId, dto);
        if (!updated)
        {
            ModelState.AddModelError("", "Failed to update user profile.");
            return View("EditProfile", dto);
        }

        return RedirectToAction("Index", "Home");
    }
}
