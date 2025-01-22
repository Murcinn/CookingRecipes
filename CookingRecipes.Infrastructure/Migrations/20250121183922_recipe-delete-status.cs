using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CookingRecipes.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class recipedeletestatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Steps",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Recipes",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Ingredients",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Steps");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Recipes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Ingredients");
        }
    }
}
