using CookingRecipes.Domain.Interfaces;
using CookingRecipes.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace CookingRecipes.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRecipeRepository, RecipeRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();


        return services;
    }

}
