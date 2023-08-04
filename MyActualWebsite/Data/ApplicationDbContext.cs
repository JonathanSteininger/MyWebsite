using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Project>()
                .HasMany(e => e.Tags)
                .WithMany(e => e.Projects)
                .UsingEntity<ProjectTag>(
                    l => l.HasOne<Tag>(e => e.Tag).WithMany(e => e.ProjectTags).HasForeignKey(e => e.TagID),
                    r => r.HasOne<Project>(e => e.Project).WithMany(e => e.ProjectTags).HasForeignKey(e => e.ProjectKey)
                );
        }

        public DbSet<MyActualWebsite.Models.Tag>? Tag { get; set; }

        public DbSet<MyActualWebsite.Models.TagCatagory>? TagCatagory { get; set; }

        public DbSet<MyActualWebsite.Models.ProjectTag>? ProjectTag { get; set; }

        public DbSet<MyActualWebsite.Models.StatBarCatagory>? StatBarCatagory { get; set; }
    }
}