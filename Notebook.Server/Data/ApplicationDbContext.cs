using Microsoft.EntityFrameworkCore;
using Notebook.Server.Domain;
using Notebook.Server.Enum;

namespace Notebook.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Note> Notebooks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RestoreUser> RestoreUserAccount { get; set; }
        public DbSet<Role> Roles { get; set; }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(option =>
            {
                option.HasKey(f => f.Email);
                option.HasMany(f => f.Notes)
                .WithOne(f => f.User)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<User>(option =>
            {
                option.HasMany(f => f.Roles)
                      .WithMany(f => f.User);
            });

            modelBuilder.Entity<Note>(option =>
            {
                option.HasKey(f => f.Id);
            });

            modelBuilder.Entity<Role>(option =>
            {
                option.HasMany(f => f.User)
                      .WithMany(f => f.Roles);

                option.HasKey(f => f.RoleName);
            });
                        
            modelBuilder.Entity<Role>().HasData(
                new Role() { RoleName = RoleName.Admin },
                new Role() { RoleName = RoleName.User });
        }
    }
}
