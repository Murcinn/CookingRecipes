using CookingRecipes.App.Interfaces;
using CookingRecipes.App.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CookingRecipes.App;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRecipeService, RecipeService>();
        services.AddScoped<ICategoryService, CategoryService>();
        return services;
    }

}

