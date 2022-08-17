using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GroceryClientApp.Models;

namespace GroceryClientApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<GroceryClientApp.Models.Grocery>? Grocery { get; set; }
        public DbSet<GroceryClientApp.Models.Customer>? Customer { get; set; }
    }
}