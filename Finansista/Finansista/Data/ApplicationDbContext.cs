using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Finansista.Models;

namespace Finansista.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Finansista.Models.Balance>? Balance { get; set; }
        public DbSet<Finansista.Models.Transaction>? Transaction { get; set; }
    }
}