using Finance_Management.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Finance_Management.Data
{
    public class DataContext : IdentityDbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Income> incomes { get; set; }
        public DbSet<Expense> expenses { get; set; }

        public DbSet<Subscription> subscriptions { get; set; }

        public DbSet<Category> categories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Expense>()
                .HasOne(c => c.Category)
                .WithMany(e => e.Expenses)
                .HasForeignKey(c => c.CategoryId);

            builder.Entity<Subscription>()
                .HasOne(c => c.Category)
                .WithMany(s => s.Subscriptions)
                .HasForeignKey(c => c.CategoryId);
        }

    }
}
