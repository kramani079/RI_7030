using Microsoft.EntityFrameworkCore;
using RI_7030.Models;

namespace RI_7030.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User>               Users               { get; set; }
        public DbSet<Employee>           Employees           { get; set; }
        public DbSet<Product>            Products            { get; set; }
        public DbSet<Order>              Orders              { get; set; }
        public DbSet<Transaction>        Transactions        { get; set; }
        public DbSet<PasswordResetToken> PasswordResetTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Unique email per user
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}
