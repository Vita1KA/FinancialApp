using Microsoft.EntityFrameworkCore;
using FinancialAppBackend.Models;

namespace FinancialAppBackend
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<Transaction> FinancialTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
