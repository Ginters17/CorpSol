using CorpSol.Models.Domain;
using CorpSol.Models.Enum;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CorpSol
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductAudit> ProductAudits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    Role = UserRoleEnum.Admin,
                    PasswordHash = "AQAAAAIAAYagAAAAEMS0gjJhtwZjabPEcvlXKFuqUG1lKvuXCwMb3fdG1PyF+I6Nd/P/Pgkj/bubazzNKQ=="
                },
                new User
                {
                    Id = 2,
                    Username = "user",
                    Role = UserRoleEnum.User,
                    PasswordHash = "AQAAAAIAAYagAAAAEMS0gjJhtwZjabPEcvlXKFuqUG1lKvuXCwMb3fdG1PyF+I6Nd/P/Pgkj/bubazzNKQ=="
                }
            );

            // Product
            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(p => p.Price)
                      .HasColumnType("decimal(18,2)");
            });
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Title = "Apple", Quantity = 10, Price = 1.99m },
                new Product { Id = 2, Title = "Ice cream", Quantity = 5, Price = 3.99m },
                new Product { Id = 3, Title = "Banana", Quantity = 20, Price = 2.00m }
            );

            // Product audit
            modelBuilder.Entity<ProductAudit>(entity =>
            {
                entity.HasKey(pa => pa.Id);
                entity.Property(pa => pa.ProductId).IsRequired();
            });
        }
    }
}
