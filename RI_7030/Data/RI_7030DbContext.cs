using Microsoft.EntityFrameworkCore;
using RI_7030.Models;

namespace RI_7030.Data
{
    public class RI_7030DbContext : DbContext
    {
        public RI_7030DbContext(DbContextOptions<RI_7030DbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee>    Employees    { get; set; }
        public DbSet<Product>     Products     { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Order>       Orders       { get; set; }
    }
}
