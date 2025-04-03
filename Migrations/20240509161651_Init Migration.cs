using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EatWellAssistant.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    categoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    categoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.categoryId);
                });

            migrationBuilder.CreateTable(
                name: "Profile",
                columns: table => new
                {
                    profileId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    age = table.Column<int>(type: "int", nullable: true),
                    gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    birthDay = table.Column<DateTime>(type: "datetime2", nullable: true),
                    height = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    width = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profile", x => x.profileId);
                });

            migrationBuilder.CreateTable(
                name: "Food",
                columns: table => new
                {
                    foodId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    foodName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    caloriesPerGram = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    imageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: true),
                    protein = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    carbs = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    fat = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    alcohol = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    categoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Food", x => x.foodId);
                    table.ForeignKey(
                        name: "FK_Food_Category_categoryId",
                        column: x => x.categoryId,
                        principalTable: "Category",
                        principalColumn: "categoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    fullName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    role = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    profileId = table.Column<int>(type: "int", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.userId);
                    table.ForeignKey(
                        name: "FK_Users_Profile_profileId",
                        column: x => x.profileId,
                        principalTable: "Profile",
                        principalColumn: "profileId");
                });

            migrationBuilder.CreateTable(
                name: "Cart",
                columns: table => new
                {
                    cartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    totalGram = table.Column<int>(type: "int", nullable: true),
                    totalCalories = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    totalProtein = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    totalCarb = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    totalFat = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    totalAlcohol = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    usersuserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cart", x => x.cartId);
                    table.ForeignKey(
                        name: "FK_Cart_Users_usersuserId",
                        column: x => x.usersuserId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlanMeal",
                columns: table => new
                {
                    planMealId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    usersuserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanMeal", x => x.planMealId);
                    table.ForeignKey(
                        name: "FK_PlanMeal_Users_usersuserId",
                        column: x => x.usersuserId,
                        principalTable: "Users",
                        principalColumn: "userId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    cartItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cartId = table.Column<int>(type: "int", nullable: true),
                    foodId = table.Column<int>(type: "int", nullable: true),
                    gram = table.Column<int>(type: "int", nullable: true),
                    totalCalories = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    totalProtein = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    totalCarb = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    totalFat = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    totalAlcohol = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.cartItemId);
                    table.ForeignKey(
                        name: "FK_CartItems_Cart_cartId",
                        column: x => x.cartId,
                        principalTable: "Cart",
                        principalColumn: "cartId");
                    table.ForeignKey(
                        name: "FK_CartItems_Food_foodId",
                        column: x => x.foodId,
                        principalTable: "Food",
                        principalColumn: "foodId");
                });

            migrationBuilder.CreateTable(
                name: "Meal",
                columns: table => new
                {
                    mealId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    planMealId = table.Column<int>(type: "int", nullable: true),
                    mealName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    totalCalories = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    totalProtein = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    totalCarb = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    totalFat = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    totalAlcohol = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meal", x => x.mealId);
                    table.ForeignKey(
                        name: "FK_Meal_PlanMeal_planMealId",
                        column: x => x.planMealId,
                        principalTable: "PlanMeal",
                        principalColumn: "planMealId");
                });

            migrationBuilder.CreateTable(
                name: "MealDetail",
                columns: table => new
                {
                    mealDetailId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mealId = table.Column<int>(type: "int", nullable: true),
                    foodId = table.Column<int>(type: "int", nullable: true),
                    gram = table.Column<int>(type: "int", nullable: false),
                    caloriesPerGram = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    totalCalories = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    totalProtein = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    totalCarb = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    totalFat = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    totalAlcohol = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealDetail", x => x.mealDetailId);
                    table.ForeignKey(
                        name: "FK_MealDetail_Food_foodId",
                        column: x => x.foodId,
                        principalTable: "Food",
                        principalColumn: "foodId");
                    table.ForeignKey(
                        name: "FK_MealDetail_Meal_mealId",
                        column: x => x.mealId,
                        principalTable: "Meal",
                        principalColumn: "mealId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cart_usersuserId",
                table: "Cart",
                column: "usersuserId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_cartId",
                table: "CartItems",
                column: "cartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_foodId",
                table: "CartItems",
                column: "foodId");

            migrationBuilder.CreateIndex(
                name: "IX_Food_categoryId",
                table: "Food",
                column: "categoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Meal_planMealId",
                table: "Meal",
                column: "planMealId");

            migrationBuilder.CreateIndex(
                name: "IX_MealDetail_foodId",
                table: "MealDetail",
                column: "foodId");

            migrationBuilder.CreateIndex(
                name: "IX_MealDetail_mealId",
                table: "MealDetail",
                column: "mealId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanMeal_usersuserId",
                table: "PlanMeal",
                column: "usersuserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_profileId",
                table: "Users",
                column: "profileId",
                unique: true,
                filter: "[profileId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "MealDetail");

            migrationBuilder.DropTable(
                name: "Cart");

            migrationBuilder.DropTable(
                name: "Food");

            migrationBuilder.DropTable(
                name: "Meal");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "PlanMeal");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Profile");
        }
    }
}
