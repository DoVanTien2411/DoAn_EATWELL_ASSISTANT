using EatWellAssistant.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace EatWellAssistant.Models
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }

        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Profile> Profile { get; set; }
        public virtual DbSet<Cart> Cart { get; set; }
        public virtual DbSet<CartItems> CartItems { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Food> Food { get; set; }
        public virtual DbSet<Meal> Meal { get; set; }
        public virtual DbSet<MealDetail> MealDetail { get; set; }
        public virtual DbSet<PlanMeal> PlanMeal { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Like> Likes { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
    }
}
