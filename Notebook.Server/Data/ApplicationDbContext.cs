using Microsoft.EntityFrameworkCore;
using Notebook.Server.Configuration;
using Notebook.Server.Domain;
using System.Reflection;

namespace Notebook.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Note> Notebooks { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<User> Users { get; set; }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //new UserConfiguration().Configure(modelBuilder.Entity<User>());
            //new NotesConfiguration().Configure(modelBuilder.Entity<Note>());
            //new AccountConfiguration().Configure(modelBuilder.Entity<Account>());

            //modelBuilder.Entity<Note>().HasKey(x => x.Id);
            //modelBuilder.Entity<Note>().Property(f => f.Id).ValueGeneratedOnAdd();
            //modelBuilder.Entity<Note>().Property(f => f.User).IsRequired();

            //modelBuilder.Entity<Account>()
            //            .Property(f => f.Email)
            //            .IsRequired();
            //modelBuilder.Entity<Account>()
            //            .HasKey(f => f.Email);
            //modelBuilder.Entity<Account>()
            //            .Property(f => f.Password)
            //            .IsRequired();

            //modelBuilder.Entity<User>()
            //.HasMany<Note>(f => f.Notes)
            //.WithOne(f => f.User)
            //.HasForeignKey(f => f.User);
        }
    }
}
