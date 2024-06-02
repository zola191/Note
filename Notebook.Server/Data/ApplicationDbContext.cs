using Microsoft.EntityFrameworkCore;
using Notebook.Server.Domain;
using Notebook.Server.Dto;

namespace Notebook.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Note> Notebooks { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RestoreAccount> RestoreAccount { get; set; }
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

                option.HasOne(f => f.Account)
                .WithOne(f => f.User)
                .HasForeignKey<User>(f => f.Email);
            });

            modelBuilder.Entity<Note>(option =>
            {
                option.HasKey(f => f.Id);
            });


            modelBuilder.Entity<Account>(option =>
            {
                option.HasKey(f => f.Email);
            });

        }
    }
}
