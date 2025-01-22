using CookingRecipes.Domain.Common;
using CookingRecipes.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace CookingRecipes.Infrastructure.Context
{
    public class CookingRecipesContext : DbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookingRecipesContext(DbContextOptions options, IHttpContextAccessor httpContextAccessor)
            : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<RecipeCategory> RecipeCategories { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is AuditableEntity &&
                            (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                switch (entityEntry.State)
                {
                    case EntityState.Added:
                        ((AuditableEntity)entityEntry.Entity).Created = DateTime.UtcNow;
                        ((AuditableEntity)entityEntry.Entity).CreatedBy = _httpContextAccessor.HttpContext?.Session?.GetInt32("UserId").ToString();
                        break;
                    case EntityState.Modified:
                        ((AuditableEntity)entityEntry.Entity).LastModified = DateTime.UtcNow;
                        ((AuditableEntity)entityEntry.Entity).LastModifiedBy = _httpContextAccessor.HttpContext?.Session?.GetInt32("UserId").ToString();
                        break;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //User-Recipe
            modelBuilder.Entity<User>()
                .HasMany(u => u.Recipes)
                .WithOne(r => r.Author)
                .HasForeignKey(r => r.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);

            //Recipe-Ingredient
            modelBuilder.Entity<Recipe>()
                .HasMany(r => r.Ingredients)
                .WithOne(i => i.Recipe)
                .HasForeignKey(i => i.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            //Recipe-Step
            modelBuilder.Entity<Recipe>()
                .HasMany(r => r.Steps)
                .WithOne(s => s.Recipe)
                .HasForeignKey(s => s.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            //Recipe-Category
            modelBuilder.Entity<RecipeCategory>()
                .HasOne(rc => rc.Recipe)
                .WithMany(r => r.RecipeCategories)
                .HasForeignKey(rc => rc.RecipeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RecipeCategory>()
                .HasOne(rc => rc.Category)
                .WithMany(c => c.RecipeCategories)
                .HasForeignKey(rc => rc.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
