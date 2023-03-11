using Microsoft.EntityFrameworkCore;

namespace E_commerceProject.Models
{
    public class ECommerceContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=.;Database=ECommerceDB;User Id=sa;Password=1234;");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cart>().HasKey(c => new { c.UserId, c.ProductId, c.Time });
            base.OnModelCreating(modelBuilder);
        }

    }
}
