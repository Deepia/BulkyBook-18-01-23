using Microsoft.EntityFrameworkCore;
using BulkyBooks.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BulkyBook.DataAccess
{
    public class ApplicationDbContext:IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CoverType> CoverType { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
