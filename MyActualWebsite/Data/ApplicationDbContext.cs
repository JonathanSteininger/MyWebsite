using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyActualWebsite.Models;

namespace MyActualWebsite.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<MyActualWebsite.Models.Experience>? Experience { get; set; }
        public DbSet<MyActualWebsite.Models.FAQ>? FAQ { get; set; }
        public DbSet<MyActualWebsite.Models.Mail>? Mail { get; set; }
        public DbSet<MyActualWebsite.Models.Project>? Project { get; set; }
        public DbSet<MyActualWebsite.Models.StatBar>? StatBar { get; set; }
    }
}